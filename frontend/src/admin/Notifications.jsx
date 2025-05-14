import { useState, useEffect } from "react";
import { Container, Box, List, Skeleton, ListItem } from "@mui/material";
import Notification from "./../components/Notification";

import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import axiosClient from "../axiosClient";

const Notifications = () => {
	const API_URL = import.meta.env.VITE_API_URL;

	const [connection, setConnection] = useState(null);
	const [loading, setIsLoading] = useState(true);
	const [notifications, setNotifications] = useState([]);

	useEffect(() => {
		const fetchNotification = async () => {
			try {
				const { data } = await axiosClient.get(
					"/Notification/Get-All-Notifications",
					{
						withCredentials: true,
					}
				);
				console.log(data)
				setNotifications(data.reverse());
			} catch (err) {
				console.log(err);
			} finally {
				setIsLoading(false);
			}
		};

		fetchNotification();
	}, []);

	useEffect(() => {
		const newConnection = new HubConnectionBuilder()
			.withUrl(`${API_URL}/Hubs/notificationHub`, {
				withCredentials: true
			})
			.withAutomaticReconnect()
			.configureLogging(LogLevel.Information)
			.build();

		setConnection(newConnection);
	}, [API_URL]);

	useEffect(() => {
			if (connection) {
				connection
					.start()
					.then(() => {
						console.log("Connected!");

						connection.on("ReceiveNotification", (msg) => {
							setNotifications((prev) => [msg, ...prev]);
						});
					})
					.catch((err) => console.error("Connection failed: ", err));
			}
		}, [connection]);

	if (loading) {
		return (
			<Container>
				<Skeleton variant="rectangle" height={400} />
			</Container>
		);
	}

	return (
		<Container>
			<List
				sx={{
					boxShadow: "0px 4px 20px rgba(0, 0, 0, 0.1)",
					borderRadius: "8px",
					width: "100%",
					margin: 0,
					padding: 0,
				}}>
				{notifications.length > 0 ? (
					notifications.map((message) => (
						<ListItem
							key={message.id}
							sx={{
								borderBottom: "1px solid  #f0f0f0",
								"&:last-child": {
									borderBottom: "none",
								},
								padding: "1rem",
								alignItems: "flex-start",
								cursor: "pointer",
							}}>
							<Box
								sx={{
									width: "100%",
									display: "flex",
									gap: "0.75rem",
									alignItems: "flex-start",
								}}>
								<Box
									sx={{
										flexGrow: 1,
										display: "flex",
										flexDirection: "column",
										gap: "0.5rem",
									}}>
									<Notification notification={message} />
								</Box>
							</Box>
						</ListItem>
					))
				) : (
					<ListItem
						disabled
						sx={{
							display: "flex",
							justifyContent: "center",
							color: "text.secondary",
							py: 3,
						}}>
						No new notifications
					</ListItem>
				)}
			</List>
		</Container>
	);
};

export default Notifications;
