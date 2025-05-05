import {
	useContext,
	useState,
	createContext,
	useCallback,
	useEffect,
	useRef,
} from "react";
import axiosClient, { registerAuthInterceptors } from "../axiosClient";

const StateContext = createContext({
	user: null,
	token: null,
	settings: { theme: "light", language: "en" },
	setUser: () => {},
	setToken: () => {},
	setSettings: () => {},
	logout: () => {},
	refreshAuth: () => Promise.resolve(),
});

// eslint-disable-next-line react/prop-types
export const ContextProvider = ({ children }) => {
	const [user, _setUser] = useState(() => {
		const u = localStorage.getItem("user");
		return u ? JSON.parse(u) : null;
	});
	const [token, _setToken] = useState(() =>
		localStorage.getItem("access_token")
	);
	const [settings, _setSettings] = useState(() => {
		const savedSettings = localStorage.getItem("user_settings");
		return savedSettings
			? JSON.parse(savedSettings)
			: { theme: "light", language: "en" };
	});
	const refreshPromise = useRef(null);

	const setToken = useCallback((newToken) => {
		_setToken(newToken);
		if (newToken) localStorage.setItem("access_token", newToken);
		else localStorage.removeItem("access_token");
	}, []);

	const setUser = useCallback((user) => {
		_setUser(user);
		if (user) localStorage.setItem("user", JSON.stringify(user));
		else localStorage.removeItem("user");
	}, []);

	const logout = useCallback(() => {
		axiosClient
			.post("/Auth/Logout", {}, { withCredentials: true })
			.finally(() => {
				setUser(null);
				setToken(null);
				
				window.location.href = "/login";
			});
	}, [setUser, setToken]);

	const setSettings = useCallback((newSettings) => {
		if (typeof newSettings === "string") {
			newSettings = { theme: newSettings };
		}

		_setSettings((prev) => {
			const updated = { ...prev, ...newSettings };
			localStorage.setItem("user_settings", JSON.stringify(updated));
			return updated;
		});
	}, []);

	

	const refreshAuth = useCallback(async () => {
		if (refreshPromise.current) return refreshPromise.current;

		refreshPromise.current = axiosClient
			.post("/users/refresh", {}, { withCredentials: true })
			.then((res) => {
				if (!res.data?.accessToken) {
					throw new Error("No access token in response");
				}
				setToken(res.data.accessToken);
				return res.data.accessToken;
			})
			.catch((err) => {
				logout();
				throw err;
			})
			.finally(() => {
				refreshPromise.current = null;
			});

		return refreshPromise.current;
	}, [logout, setToken]);

	useEffect(() => {
		registerAuthInterceptors({ refreshAuth, logout });
	}, [refreshAuth, logout]);

	return (
		<StateContext.Provider
			value={{
				user,
				token,
				settings,
				setUser,
				setToken,
				setSettings,
				logout,
				refreshAuth,
			}}>
			{children}
		</StateContext.Provider>
	);
};

// eslint-disable-next-line react-refresh/only-export-components
export const useStateContext = () => useContext(StateContext);
