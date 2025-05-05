import { createBrowserRouter } from "react-router-dom";
import Login from "./views/login";
import Register from "./views/register";
import DefaultLayout from "./components/DefaultLayout";
import GuestLayout from "./components/GuestLayout";
import Home from "./views/home";
import NotFoundPage from "./views/not_found";
// import MyAccount from "./views/myAccount";
import { CarPostDetails } from "./views/CarPostDetails";
import AdminLayout from "./components/adminLayout";
import AdminDashboard from "./admin/AdminDashborad";
import PendingUsers from "./admin/PendingUsers";
import { PendingPosts } from "./admin/PendingPosts";
import { OwnerPosts } from "./views/OwnerPosts";
import { AddPostForm } from "./views/AddPostForm";
import { EditPostForm } from "./views/EditPostForm";
import Proposals from './views/Proposals';
import AddOfferForm from './views/AddOfferForm';
import Notifications from './admin/Notifications';

const router = createBrowserRouter([
	{
		path: "/",
		element: <DefaultLayout />,
		children: [
			// { path: "/account", element: <MyAccount /> },
			{ path: "/car", element: <CarPostDetails /> },
			{ path: "/posts", element: <OwnerPosts /> },
			{ path: "/create-post", element: <AddPostForm /> },
			{ path: "/edit-post/:id", element: <EditPostForm /> },
			{ path: "/proposals/:postId", element: <Proposals /> },
			{ path: "/add-offer/:postId", element: <AddOfferForm /> },
			{ path: "*", element: <NotFoundPage /> },
		],
	},
	{
		path: "/",
		element: <GuestLayout />,
		children: [
			{ path: "/", element: <Home /> },
			{ path: "/home", element: <Home /> },
			{ path: "/login", element: <Login /> },
			{ path: "/register", element: <Register /> },
			{ path: "*", element: <NotFoundPage /> },
		],
	},
	{
		path: "/",
		element: <AdminLayout />,
		children: [
			{ path: "/admin", element: <AdminDashboard /> },
			{ path: "/admin/dashboard", element: <AdminDashboard /> },
			{ path: "/admin/pending-users", element: <PendingUsers /> },
			{ path: "/admin/pending-posts", element: <PendingPosts /> },
			{ path: "/admin/notification", element: <Notifications /> },
		],
	},
]);

export default router;
