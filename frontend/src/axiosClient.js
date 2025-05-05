import axios from "axios";

const baseUrl = import.meta.env.VITE_API_URL;

const axiosClient = axios.create({
	baseURL: `${baseUrl}/api`,
	headers: { "Content-Type": "application/json" },
	withCredentials: true,
});

let refreshAuthCallback = null;
let logoutCallback = null;

export const registerAuthInterceptors = ({ refreshAuth, logout }) => {
	refreshAuthCallback = refreshAuth;
	logoutCallback = logout;

	axiosClient.interceptors.request.use((config) => {
		const token = localStorage.getItem("access_token");
		if (token) config.headers.Authorization = `Bearer ${token}`;
		return config;
	});

	axiosClient.interceptors.response.use(
		(res) => res,
		async (error) => {
			const originalRequest = error.config;
			if (error.response?.status === 401 && !originalRequest._retry) {
				originalRequest._retry = true;
				try {
					const newToken = await refreshAuthCallback();
					originalRequest.headers.Authorization = `Bearer ${newToken}`;
					return axiosClient(originalRequest);
				} catch {
					logoutCallback();
					return Promise.reject(error);
				}
			}
			return Promise.reject(error);
		}
	);
};

export default axiosClient;