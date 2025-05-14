import {
	Typography,
	Container,
	Skeleton,
	Table,
	TableBody,
	TableCell,
	TableContainer,
	TableHead,
	TableRow,
	Paper,
	Dialog,
	DialogTitle,
	DialogContent,
	DialogActions,
	Avatar,
} from "@mui/material";
import { useState, useEffect } from "react";
import { FormButton } from "../components/StyledComponents";
import axiosClient from "./../axiosClient";
import { useStateContext } from "../context/contextprovider";

import { useNavigate } from "react-router";

export const OwnerPosts = () => {
	const [posts, setPosts] = useState([]);
	const [loading, setIsloading] = useState(true);
	const [openDeleteDialog, setOpenDeleteDialog] = useState(false);
	const [selectedPostId, setSelectedPostId] = useState(null);

	const { user } = useStateContext();

	const navigate = useNavigate();

	useEffect(() => {
		const fetchData = async () => {
			try {
				const { data } = await axiosClient.get(
					`/Post/Get-User-Posts?userId=${user.id}`,
					{ withCredentials: true }
				);

				setPosts(data.items);
			} catch (err) {
				console.log(err.response.data.message || "error loading posts");
			} finally {
				setIsloading(false);
			}
		};

		fetchData();
	}, [user.id]);

	if (loading) {
		return (
			<Container>
				<Skeleton variant="rounded" height={300} />
			</Container>
		);
	}

	const handleDeleteConfirm = async () => {
		try {
			await axiosClient.delete(`/Post/Delete-Post/${selectedPostId}`, {
				withCredentials: true,
			});
			setPosts((prev) =>
				prev.filter((post) => post.id !== selectedPostId)
			);
			setSelectedPostId(null);
		} catch (error) {
			console.error("Failed to delete affiliation:", error);
		} finally {
			setOpenDeleteDialog(false);
		}
	};

	return (
		<Container>
			<Typography variant="h4" gutterBottom>
				Posts
			</Typography>
			<div style={{ marginBottom: 16 }}>
				<FormButton
					variant="contained"
					onClick={() => navigate("/create-post")}
					sx={{ mb: 2, mr: 2 }}>
					Add Post
				</FormButton>
				<FormButton
					variant="outlined"
					onClick={() =>
						navigate("/car", {
							state: {
								carId: selectedPostId,
							},
						})
					}
					disabled={!selectedPostId}
					sx={{ mb: 2, mr: 2 }}>
					Post details
				</FormButton>
				<FormButton
					variant="outlined"
					onClick={() => navigate(`/edit-post/${selectedPostId}`)}
					disabled={!selectedPostId}
					sx={{ mb: 2, mr: 2 }}>
					Edit Post
				</FormButton>
				<FormButton
					variant="outlined"
					onClick={() => navigate(`/proposals/${selectedPostId}`)}
					disabled={!selectedPostId}
					sx={{ mb: 2, mr: 2 }}>
					Proposals
				</FormButton>
				<FormButton
					variant="outlined"
					color="error"
					onClick={() => setOpenDeleteDialog(true)}
					disabled={!selectedPostId}
					sx={{ mb: 2 }}>
					Delete Post
				</FormButton>
			</div>

			<TableContainer component={Paper}>
				<Table>
					<TableHead>
						<TableRow>
							<TableCell>Car Type</TableCell>
							<TableCell>Brand</TableCell>
							<TableCell>Model</TableCell>
							<TableCell>Price</TableCell>
							<TableCell>Image</TableCell>
						</TableRow>
					</TableHead>
					<TableBody>
						{posts.map((post) => (
							<TableRow
								key={post.id}
								onClick={() => {
									setSelectedPostId(post.id);
								}}
								selected={selectedPostId === post.id}
								hover
								sx={{ cursor: "pointer" }}>
								<TableCell>{post.carType}</TableCell>
								<TableCell>{post.brand}</TableCell>
								<TableCell>{post.model}</TableCell>
								<TableCell>{post.rentalPrice}</TableCell>
								<TableCell>
									{post.imageUrls[0] ? (
										<Avatar src={post.imageUrls[0]} />
									) : (
										<Avatar src={null} />
									)}
								</TableCell>
							</TableRow>
						))}
					</TableBody>
				</Table>
			</TableContainer>

			<Dialog
				open={openDeleteDialog}
				onClose={() => setOpenDeleteDialog(false)}>
				<DialogTitle>Confirm Delete</DialogTitle>
				<DialogContent>
					Are you sure you want to delete this post?
				</DialogContent>
				<DialogActions>
					<FormButton onClick={() => setOpenDeleteDialog(false)}>
						Cancel
					</FormButton>
					<FormButton onClick={handleDeleteConfirm} color="error" autoFocus>
						Delete
					</FormButton>
				</DialogActions>
			</Dialog>
		</Container>
	);
};
