/* eslint-disable react/prop-types */
import { useState, useMemo } from "react";
import { extendTheme, styled } from "@mui/material/styles";
import PendingActionsIcon from "@mui/icons-material/PendingActions";
import DashboardIcon from "@mui/icons-material/Dashboard";
import PeopleIcon from "@mui/icons-material/People";
import { AppProvider } from "@toolpad/core/AppProvider";
import { DashboardLayout } from "@toolpad/core/DashboardLayout";
import { PageContainer } from "@toolpad/core/PageContainer";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import AdminDashboard from "../admin/AdminDashborad";
import PendingUsers from "../admin/PendingUsers";
import HomeIcon from "@mui/icons-material/Home";
import { PendingPosts } from "./../admin/PendingPosts";
import NotificationsIcon from "@mui/icons-material/Notifications";
import Notifications from "./../admin/Notifications";
import { Navigate } from "react-router";

const NAVIGATION = [
	{
		segment: "",
		title: "Home",
		icon: <HomeIcon />,
	},
	{
		kind: "header",
		title: "Main items",
	},
	{
		segment: "dashboard",
		title: "Dashboard",
		icon: <DashboardIcon />,
	},
	{
		segment: "pending_users",
		title: "Pending Users",
		icon: <PeopleIcon />,
	},
	{
		segment: "pending_posts",
		title: "pending Posts",
		icon: <PendingActionsIcon />,
	},
	{
		segment: "notification",
		title: "Notifications",
		icon: <NotificationsIcon />,
	},
];

const demoTheme = extendTheme({
	colorSchemes: { light: true, dark: true },
	colorSchemeSelector: "class",
	breakpoints: {
		values: {
			xs: 0,
			sm: 600,
			md: 600,
			lg: 1200,
			xl: 1536,
		},
	},
});

function useDemoRouter(initialPath) {
	const [pathname, setPathname] = useState(initialPath);

	const router = useMemo(() => {
		return {
			pathname,
			searchParams: new URLSearchParams(),
			navigate: (path) => setPathname(String(path)),
		};
	}, [pathname]);

	return router;
}

// eslint-disable-next-line no-unused-vars
const Skeleton = styled("div")(({ theme, height }) => ({
	backgroundColor: theme.palette.action.hover,
	borderRadius: theme.shape.borderRadius,
	height,
	content: '" "',
}));

function DemoPageContent({ pathname }) {
	let content = null;

	if (pathname === "/dashboard") {
		content = <AdminDashboard />;
	} else if (pathname === "/pending_users") {
		content = <PendingUsers />;
	} else if (pathname === "/pending_posts") {
		content = <PendingPosts />;
	} else if (pathname === "/notification") {
		content = <Notifications />;
	} else if (pathname === "/") {
		content = <Navigate to="/home" />;
	} else {
		content = <Typography>Unknown page</Typography>;
	}

	return (
		<Box
			sx={{
				py: 4,
				display: "flex",
				flexDirection: "column",
				alignItems: "center",
				textAlign: "center",
			}}>
			{content}
		</Box>
	);
}

export default function Dashboard(props) {
	const { window } = props;

	const router = useDemoRouter("/dashboard");

	const demoWindow = window ? window() : undefined;

	return (
		<AppProvider
			navigation={NAVIGATION}
			router={router}
			theme={demoTheme}
			window={demoWindow}
			branding={{
				title: "Car Share",
				homeUrl: "/dashboard",
			}}>
			<DashboardLayout>
				<PageContainer>
					<DemoPageContent
						pathname={router.pathname}
						navigate={router.navigate}
					/>{" "}
				</PageContainer>
			</DashboardLayout>
		</AppProvider>
	);
}
