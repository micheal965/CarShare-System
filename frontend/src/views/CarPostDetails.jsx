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

	const [loading, setLoading] = useState(true);
	const [err, setErr] = useState(null);
	const [post, setPost] = useState(null);
	const [averageRating, setAverageRating] = useState("0");

	const [newRatingStars, setNewRatingStars] = useState(0);
	const [newRatingComment, setNewRatingComment] = useState("");
	const [isSubmitting, setIsSubmitting] = useState(false);
	const [formError, setFormError] = useState(null);

	useEffect(() => {
		const fetchPostData = () => {
			axiosClient
				.get(`/Post/Get-Post/${carId}`)
				.then(({ data }) => {
					console.log(data);
					setPost(data);
					calculateAvaregeFeedBack(data);
				})
				.catch((err) => {
					setErr(err.message);
					console.log(err);
				})
				.finally(() => setLoading(false));
		};

		fetchPostData();
	}, [carId]);

	const calculateAvaregeFeedBack = (post) => {
		if (!post || post.feedbacks.length == 0) {
			setAverageRating("0");
		}

		const feedbacks = post?.feedbacks || [];
		const feedbackCount = feedbacks.length;

		let feedbackSum = 0;
		post.feedbacks.forEach((feedback) => (feedbackSum += feedback.rating));
		const average = feedbackSum > 0 ? feedbackSum / feedbackCount : 0;

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
			updateRatingsState([...post.feedbacks, newRating]);
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
									value={review.rating}
									precision={0.5}
									readOnly
									size="small"
								/>
								<Typography
									component="span"
									variant="body2"
									color="text.secondary"
									sx={{ display: "flex" }}>
									{review.feedbackText || "Anonymous"}
								</Typography>
							</>
						}
						secondary={
							<>
								<Typography
									component="div"
									variant="caption"
									color="text.secondary">
									{
										new Date(review.submmitedAt)
											.toLocaleDateString()
											.split("T")[0]
									}
								</Typography>
							</>
						}
					/>
				</ListItem>
				{index < post.feedbacks.length - 1 && <Divider />}
			</React.Fragment>
		);
	};

	return (
		<Container>
			<Paper elevation={3} sx={{ padding: 3, margin: 2 }}>
				<Stack direction="row" spacing={1} mb={2}>
					{post.imageUrls.length > 0 ? (
						post.imageUrls.map((image, index) => (
							<Avatar
								key={index}
								src={image}
								variant="rounded"
								sx={{ width: 60, height: 60 }}
							/>
						))
					) : (
						<Avatar
							src={null}
							variant="rounded"
							sx={{ width: 60, height: 60 }}
						/>
					)}
				</Stack>

				<Typography variant="h5" gutterBottom>
					{post.title}
					<Chip
						icon={<CheckCircle />}
						label={post.rentalStatus}
						color={post.rentalStatus === "Accepted" ? "success" : "error"}
						sx={{ ml: 2 }}
					/>
				</Typography>

				<Box mb={3}>
					<Grid2 container spacing={3}>
						<Grid2 item md={6}>
							<Typography variant="subtitle1" gutterBottom>
								<strong>Car Type:</strong> {post.carType}
							</Typography>
							<Typography variant="subtitle1" gutterBottom>
								<strong>Brand:</strong> {post.brand}
							</Typography>
							<Typography variant="subtitle1" gutterBottom>
								<strong>Model:</strong> {post.model}
							</Typography>
							<Typography variant="subtitle1" gutterBottom>
								<strong>Year:</strong> {post.year}
							</Typography>
							<Typography variant="subtitle1" gutterBottom>
								<strong>Transmission:</strong> {post.transmission}
							</Typography>
							<Typography variant="subtitle1" gutterBottom>
								<strong>Description:</strong> {post.description}
							</Typography>
							<Typography variant="subtitle1" gutterBottom>
								<strong>Rental Price:</strong> {post.rentalPrice}
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
								<strong>Availability Start:</strong>{" "}
								{post.availabilityStart.split("T")[0]}
							</Typography>
							<Typography variant="subtitle1" gutterBottom>
								<strong>Availability End:</strong>{" "}
								{post.availabilityEnd.split("T")[0]}
							</Typography>
							<Typography variant="subtitle1" gutterBottom>
								<strong>Location:</strong> {post.location}
							</Typography>
							<FormButton
								disabled={post.rentalStatus !== "Accepted"}
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
								{averageRating || !Number.isNaN(averageRating)
									? averageRating
									: "0"}
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
								{post.feedbacks.length} reviews
							</Typography>
						</Grid2>
					</Grid2>
					{post.feedbacks && post.feedbacks.length > 0 ? (
						<List>
							{post.feedbacks.map((review, index) =>
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