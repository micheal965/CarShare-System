import {
	Container,
	Typography,
	AlertTitle,
	Alert,
	Skeleton,
	Paper,
	Chip,
	Box,
	Grid2,
	Divider,
	Stack,
	List,
	Rating,
	Avatar,
	ListItemAvatar,
	ListItem,
	ListItemText,
} from "@mui/material";
import { useLocation, useNavigate } from "react-router-dom";
import React, { useState, useEffect } from "react";
import axiosClient from "../axiosClient";
import { CheckCircle, AccessTime, Star, Person } from "@mui/icons-material";
import { FormButton, FormTextField } from "./../components/StyledComponents";

export const CarPostDetails = () => {
	const location = useLocation();
	const navigate = useNavigate();
	const { carId } = location.state;

	const [loading, setLoading] = useState(false);
	const [err, setErr] = useState(null);
	const [post, setPost] = useState(null);
	const [averageRating, setAverageRating] = useState("0");

	// New rating form state
	const [newRatingStars, setNewRatingStars] = useState(0);
	const [newRatingComment, setNewRatingComment] = useState("");
	const [isSubmitting, setIsSubmitting] = useState(false);
	const [formError, setFormError] = useState(null);

	useEffect(() => {
		const fetchPostData = () => {
			axiosClient
				.get(`/Post/Get-Post/${carId}`)
				.then(({ data }) => {
					setPost(data);
					calculateAvaregeFeedBack(data);
					console.log(data);
				})
				.catch((err) => {
					setErr(err.message);
				})
				.finally(() => setLoading(false));
		};

		fetchPostData();
	}, [carId]);

	const calculateAvaregeFeedBack = (post) => {
		if (!post) {
			setAverageRating("0");
		}

		const feedbackCount = post.Feedbacks.length;
		let feedbackSum = 0;
		post.Feedbacks.forEach((feedback) => (feedbackSum += feedback.Rating));
		const average = feedbackSum / feedbackCount;

		setAverageRating(`${average.toFixed(2)}`);
	};

	if (loading) {
		return (
			<Container>
				<Skeleton variant="rectangular" height={500} sx={{ marginY: "1rem" }} />
			</Container>
		);
	}

	if (err) {
		return (
			<Container
				sx={{
					position: "absolute",
					top: "50%",
					left: "50%",
					transform: "translate(-50%, -50%)",
					display: "flex",
					width: "auto",
				}}>
				<Alert severity="error" sx={{ marginBottom: "1rem", width: 150 }}>
					<AlertTitle>Error</AlertTitle>
					{err}
				</Alert>
			</Container>
		);
	}

	const updateRatingsState = (updatedBreakdown) => {
		const totalStars = updatedBreakdown.reduce((sum, r) => sum + r.stars, 0);
		const count = updatedBreakdown.length;
		const average = count > 0 ? totalStars / count : 0;
		setAverageRating(`${average.toFixed(2)}`);
	};

	const handleRatingSubmit = async (e) => {
		e.preventDefault();
		setIsSubmitting(true);
		setFormError(null);
		try {
			const payload = {
				carPostId: carId,
				rating: newRatingStars,
				feedback: newRatingComment.trim(),
			};
			const { data } = await axiosClient.post(
				"/Rental/Submit-Feedback",
				payload,
				{ withCredentials: true }
			);
			const newRating = data;
			updateRatingsState([...post.Feedbacks, newRating]);
			setNewRatingStars(0);
			setNewRatingComment("");
		} catch (error) {
			setFormError(error.message || "Failed to submit rating");
		} finally {
			setIsSubmitting(false);
		}
	};

	const renderRatingReview = (review, index) => {

		return (
			<React.Fragment key={index}>
				<ListItem alignItems="flex-start">
					<ListItemAvatar>
						<Avatar src={review.user?.picture}>
							{review.user ? review.user.name.split(" ")[1][0] : <Person />}
						</Avatar>
					</ListItemAvatar>
					<ListItemText
						primary={
							<>
								<Rating
									value={review.stars}
									precision={0.5}
									readOnly
									size="small"
								/>
								<Typography
									component="span"
									variant="body2"
									color="text.secondary"
									sx={{ display: "flex" }}>
									{review.user?.name || "Anonymous"}
								</Typography>
							</>
						}
						secondary={
							<>
								<Typography
									component="div"
									variant="caption"
									color="text.secondary">
									{new Date(review.createdAt).toLocaleDateString()}
								</Typography>
							</>
						}
					/>
				</ListItem>
				{index < post.Feedbacks.length - 1 && <Divider />}
			</React.Fragment>
		);
	};

	return (
		<Container>
			<Paper elevation={3} sx={{ padding: 3, margin: 2 }}>
				<Stack direction="row" spacing={1} mb={2}>
					{post.ImageUrls.map((image, index) => (
						<Avatar
							key={index}
							src={image}
							variant="rounded"
							sx={{ width: 60, height: 60 }}
						/>
					))}
				</Stack>

				<Typography variant="h5" gutterBottom>
					{post.Title}
					<Chip
						icon={<CheckCircle />}
						label={post.RentalStatus}
						color={post.RentalStatus === "Available" ? "success" : "error"}
						sx={{ ml: 2 }}
					/>
				</Typography>

				<Box mb={3}>
					<Grid2 container spacing={3}>
						<Grid2 item md={6}>
							<Typography variant="subtitle1" gutterBottom>
								<strong>Car Type:</strong> {post.CarType}
							</Typography>
							<Typography variant="subtitle1" gutterBottom>
								<strong>Brand:</strong> {post.Brand}
							</Typography>
							<Typography variant="subtitle1" gutterBottom>
								<strong>Model:</strong> {post.Model}
							</Typography>
							<Typography variant="subtitle1" gutterBottom>
								<strong>Year:</strong> {post.Year}
							</Typography>
							<Typography variant="subtitle1" gutterBottom>
								<strong>Transmission:</strong> {post.Transmission}
							</Typography>
							<Typography variant="subtitle1" gutterBottom>
								<strong>Description:</strong> {post.Description}
							</Typography>
							<Typography variant="subtitle1" gutterBottom>
								<strong>Rental Price:</strong> {post.RentalPrice}
							</Typography>
						</Grid2>
					</Grid2>
				</Box>
				<Divider sx={{ my: 2 }} />

				<Box mb={3}>
					<Typography variant="h6" gutterBottom>
						<AccessTime sx={{ verticalAlign: "middle", mr: 1 }} />
						Schedule
					</Typography>
					<Grid2 container spacing={3}>
						<Grid2 item md={6}>
							<Typography variant="subtitle1" gutterBottom>
								<strong>Availability Start:</strong> {post.AvailabilityStart}
							</Typography>
							<Typography variant="subtitle1" gutterBottom>
								<strong>Availability End:</strong> {post.AvailabilityEnd}
							</Typography>
							<Typography variant="subtitle1" gutterBottom>
								<strong>Location:</strong> {post.Location}
							</Typography>
							<FormButton
								disabled={post.RentalStatus !== "Available"}
								variant="contained"
								className="btn btn-black"
								onClick={() => navigate(`/add-offer/${carId}`)}>
								Rent
							</FormButton>
						</Grid2>
					</Grid2>
				</Box>
				<Divider sx={{ my: 2 }} />

				<Box mt={3}>
					<Typography variant="h6" gutterBottom>
						<Star sx={{ verticalAlign: "middle", mr: 1 }} />
						Ratings & Reviews
					</Typography>

					<Grid2 container spacing={3} alignItems="center" mb={3}>
						<Grid2 item>
							<Typography variant="h2" component="div">
								{averageRating}
							</Typography>
						</Grid2>
						<Grid2 item>
							<Rating
								value={averageRating}
								precision={0.5}
								readOnly
								size="large"
							/>
							<Typography variant="body2" color="text.secondary">
								{post.Feedbacks.length} reviews
							</Typography>
						</Grid2>
					</Grid2>
					{post.Feedbacks && post.Feedbacks.length > 0 ? (
						<List>
							{post.Feedbacks.map((review, index) =>
								renderRatingReview(review, index)
							)}
						</List>
					) : (
						<Typography variant="body2" color="text.secondary">
							No reviews yet
						</Typography>
					)}
				</Box>
				{/* New Rating Form */}
				<form onSubmit={handleRatingSubmit}>
					<Box mt={3}>
						<Typography variant="h6" gutterBottom>
							Add Your Rating
						</Typography>
						<Stack spacing={2}>
							<Rating
								name="new-rating"
								value={newRatingStars}
								precision={0.5}
								onChange={(e, newValue) => setNewRatingStars(newValue)}
							/>
							<FormTextField
								label="Comment"
								variant="outlined"
								fullWidth
								multiline
								rows={3}
								value={newRatingComment}
								onChange={(e) => setNewRatingComment(e.target.value)}
							/>
							{formError && <Alert severity="error">{formError}</Alert>}
							<FormButton variant="contained" type="submit">
								{isSubmitting ? "Submitting..." : "Submit Rating"}
							</FormButton>
						</Stack>
					</Box>
				</form>
			</Paper>
		</Container>
	);
};
