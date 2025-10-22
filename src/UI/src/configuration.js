export const configuration = {
	backendBaseUrl: (String(`${import.meta.env.VITE_BACKEND_BASE_URL_HTTPS || import.meta.env.VITE_BACKEND_BASE_URL_HTTP}`)).concat("/api"),
	apiTimeout: Number(import.meta.env.VITE_API_TIMEOUT),
	appId: String(import.meta.env.VITE_APP_ID)
};