import {
	Avatar,
	Card,
	CardContent,
	Container,
	Skeleton,
	Typography,
	Alert,
	AlertTitle,
	Box,
} from "@mui/material";
import { useEffect, useState } from "react";
import axiosClient from "./../axiosClient";
import { FormButton } from "../components/StyledComponents";

const PendingUsers = () => {
	const [pendingUsers, setPendingUsers] = useState([]);
	const [loading, setLoading] = useState(true);
	const [buttonLoading, setButtonLoading] = useState(false);
	const [msg, setMsg] = useState(null);
	const [err, setErr] = useState(null);

	const handleApprove = async (userId) => {
		setButtonLoading(true);
		try {
			const response = await axiosClient.put(
				"/Auth/Approve-Or-Reject-Account",
				{
					userId,
					IsApproved: true,
				}
			);
			setPendingUsers((prev) => prev.filter((user) => user.id !== userId));
			setMsg(response.data?.message || "Account approved successfully!");
		} catch (err) {
			setErr(err.message || "err Approving Account");
		} finally {
			setButtonLoading(false);
		}
	};

	const handleReject = async (userId) => {
		setButtonLoading(true);
		try {
			const response = await axiosClient.put(
				"/Auth/Approve-Or-Reject-Account",
				{
					userId,
					IsApproved: false,
				}
			);
			setPendingUsers((prev) => prev.filter((user) => user.id !== userId));
			setMsg(response.data?.message || "Account rejected successfully!");
		} catch (err) {
			setErr(err.message || "error rejecting Account");
		} finally {
			setButtonLoading(false);
		}
	};

	useEffect(() => {
		const getPendingUsers = async () => {
			try {
				const { data } = await axiosClient.get(
					"/Auth/Get-Pending-And-Rejected-Accounts",
					{
						withCredentials: true,
					}
				);
				console.log(data)

				setPendingUsers(data);
			} catch (err) {
				setErr(err.message || "error fetching data");
			} finally {
				setLoading(false);
			}
		};

		getPendingUsers();
	}, []);

	if (loading) {
		return (
			<Container sx={{ marginY: "1rem" }}>
				<Skeleton variant="rectangular" height={205} sx={{ marginY: "1rem" }} />
				<Skeleton variant="rectangular" height={205} sx={{ marginY: "1rem" }} />
				<Skeleton variant="rectangular" height={205} sx={{ marginY: "1rem" }} />
			</Container>
		);
	}

	return (
		<Container sx={{ marginY: "1rem" }}>
			<Typography variant="h5">Pending Users</Typography>
			{err && (
				<Alert severity="error" sx={{ marginBottom: "1rem" }}>
					<AlertTitle>Error</AlertTitle>
					{err}
				</Alert>
			)}
			{msg && (
				<Alert severity="success" sx={{ marginBottom: "1rem" }}>
					<AlertTitle>Success</AlertTitle>
					{msg}
				</Alert>
			)}
			{pendingUsers.length > 0 ? (
				pendingUsers.map((user) => (
					<Card variant="outlined" sx={{ marginY: "1rem" }} key={user.id}>
						<CardContent>
							<Box
								sx={{
									display: "flex",
									gap: 3,
									alignItems: "flex-start",
									mb: 2,
								}}>
								<Avatar
									src={user.profilePicture ? user.profilePicture : null}
									alt={user.email}
									style={{
										width: 80,
										height: 80,
										borderRadius: "50%",
										objectFit: "cover",
										border: "2px solid #e0e0e0",
									}}
								/>
							</Box>
							<Typography variant="h6" gutterBottom>
								username: {user.username}
							</Typography>
							<Typography color="text.secondary">
								Email: {user.email}
							</Typography>

							<Box sx={{ display: "flex", gap: 2, mt: 2 }}>
								<FormButton
									variant="contained"
									color="success"
									onClick={() => handleApprove(user.id)}
									disabled={buttonLoading}>
									Approve
								</FormButton>
								<FormButton
									variant="outlined"
									color="error"
									onClick={() => handleReject(user.id)}
									disabled={buttonLoading}>
									Reject
								</FormButton>
							</Box>
						</CardContent>
					</Card>
				))
			) : (
				<Typography
					variant="h5"
					component="div"
					sx={{
						position: "absolute",
						top: "50%",
						left: "50%",
						transform: "translate(-50%, -50%)",
						display: "flex",
						width: "auto",
					}}>
					No Pending users!
				</Typography>
			)}
		</Container>
	);
};

export default PendingUsers;
