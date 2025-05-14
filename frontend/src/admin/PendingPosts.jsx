import { useState, useEffect } from "react";
import {
	Card,
	Container,
	Skeleton,
	Typography,
	Alert,
	AlertTitle,
	CardActions,
} from "@mui/material";
import { FormButton } from "../components/StyledComponents";
import axiosClient from "./../axiosClient";
import { CarPost } from "./../components/CarPost";

export const PendingPosts = () => {
	const [pendingPosts, setPendingPosts] = useState([]);
	const [loading, setLoading] = useState(true);
	const [buttonLoading, setButtonLoading] = useState(false);
	const [msg, setMsg] = useState(null);
	const [err, setErr] = useState(null);

	useEffect(() => {
		const getPendingUsers = async () => {
			try {
				const { data } = await axiosClient.get("/Post/Get-Pending-Posts", {
					withCredentials: true,
				});
				console.log(data);

				setPendingPosts(data);
			} catch (err) {
				setErr(err.message || "Failed to fetch posts");
			} finally {
				setLoading(false);
			}
		};

		getPendingUsers();
	}, []);

	const managePosts = async (isAccepted, postId) => {
		setButtonLoading(true);
		try {
			const payload = {
				postId,
				isAccepted,
			};

			await axiosClient.post("/Post/Manage-Post", payload, {
				withCredentials: true,
			});
			const message =
				"Post " + isAccepted ? "Approved" : "Rejected" + " Successfully";
				setPendingPosts((prev) => prev.filter((post) => post.id !== postId));
			setMsg(message);
		} catch (err) {
			setErr(err.message || "Something went wrong, please try again later :(");
		} finally {
			setButtonLoading(false);
		}
	};

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
			{pendingPosts.length > 0 ? (
				pendingPosts.map((post, index) => (
					<Card variant="outlined" sx={{ marginY: "1rem" }} key={index}>
						<CarPost car={post} />
						<CardActions sx={{ display: "flex", gap: 2, mt: 2 }}>
							<FormButton
								variant="contained"
								color="success"
								onClick={() => managePosts(true, post.id)}
								disabled={buttonLoading}>
								Approve
							</FormButton>
							<FormButton
								variant="outlined"
								color="error"
								onClick={() => managePosts(false, post.id)}>
								Reject
							</FormButton>
						</CardActions>
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
					No Pending Posts!
				</Typography>
			)}
		</Container>
	);
};