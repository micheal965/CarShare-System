import { RouterProvider } from "react-router-dom";
import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import router from "./router";
import { ContextProvider } from "./context/contextprovider";
import { ThemeWrapper } from "./components/ThemeWrapper";

createRoot(document.getElementById("root")).render(
	<StrictMode>
		<ContextProvider>
			<ThemeWrapper>
				<RouterProvider router={router} />
			</ThemeWrapper>
		</ContextProvider>
	</StrictMode>
);
