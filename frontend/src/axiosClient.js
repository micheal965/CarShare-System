import axios from "axios";
import Cookies from "js-cookie";

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
		const token = Cookies.get("access_token");
		if (token) {
			config.headers.Authorization = `Bearer ${token}`;
		}
		return config;
	});

	axiosClient.interceptors.response.use(
		(res) => res,
		async (error) => {
			const originalRequest = error.config;
			if ((error.response?.status === 401 || error.response?.status === 403) && !originalRequest._retry) {
				originalRequest._retry = true;
				try {
					const newToken = await refreshAuthCallback();
					originalRequest.headers.Authorization = `Bearer ${newToken}`;
					return axiosClient(originalRequest);
				} catch (error) {
					console.log("Refresh Token Expired");
					logoutCallback();
					return Promise.reject(error);
				}
			}
			return Promise.reject(error);
		}
	);
};

export default axiosClient;
