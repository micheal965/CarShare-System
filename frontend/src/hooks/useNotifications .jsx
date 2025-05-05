import { useEffect, useState } from "react";
import { useStateContext } from "../context/contextprovider";

export const useNotifications = () => {
	const { socket, user } = useStateContext();
	const [notifications, setNotifications] = useState([]);
	const [unreadCount, setUnreadCount] = useState(0);

	useEffect(() => {
		if (!socket || !user) return;

		const notificationListener = (notification) => {
			setNotifications((prev) => [notification, ...prev]);
			setUnreadCount((prev) => prev + 1);
		};

		socket.on(`notification:${user.id}`, notificationListener);

		return () => {
			socket.off(`notification:${user.id}`, notificationListener);
		};
	}, [socket, user]);

	return { notifications, unreadCount, setNotifications, setUnreadCount };
};
