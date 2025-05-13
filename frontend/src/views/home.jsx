import { useState, useEffect } from "react";
import {
	Container,
	Box,
	Select,
	MenuItem,
	FormControl,
	InputLabel,
	Skeleton,
	Pagination,
	CardContent,
} from "@mui/material";
import Card from "@mui/material/Card";
import CardActions from "@mui/material/CardActions";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import { useNavigate } from "react-router-dom";
import { CarPost } from "../components/CarPost";
import axiosClient from "../axiosClient";
import MiniDrawer from "../components/Drawer";

export default function Home() {
	const navigate = useNavigate();
	const [loading, setLoading] = useState(true);
	const [posts, setPosts] = useState([]);

	//pagination
	const [page, setPage] = useState(1);
	const [pageSize, setPageSize] = useState(5);
	const [totalCount, setTotalCount] = useState(0);

	useEffect(() => {
		const fetchPostsData = () => {
			axiosClient
				.get("/Post/Get-All-Posts", {
					withCredentials: true,
				})
				.then(({ data }) => {
					console.log(data);
					setPosts(data.items);
					setTotalCount(data.items.length);
				})
				.catch((error) => {
					console.log(error);
				})
				.finally(() => setLoading(false));
		};

		fetchPostsData();
	}, []);

	const handleButtonClick = (id) => {
		navigate(`/car`, {
			state: {
				carId: id,
			},
		});
	};

	if (loading) {
		return (
			<Container>
				{[1, 2, 3].map((i) => (
					<Skeleton
						key={i}
						variant="rectangular"
						height={205}
						sx={{ marginY: "1rem" }}
					/>
				))}
			</Container>
		);
	}

	return (
		<Box id="defaultLayout" sx={{ display: "block" }}>
			<div className="content">
				<header>
					<MiniDrawer></MiniDrawer>
					<Container sx={{ mt: 10, pl: { sm: 10, xs: 6 }, pr: { xs: 0 } }}>
						<Container>
							<Box
								display="flex"
								alignItems="center"
								justifyContent="space-between"
								my={2}>
								<FormControl size="small">
									<InputLabel id="page-size-label">Per page</InputLabel>
									<Select
										labelId="page-size-label"
										value={pageSize}
										label="Per page"
										sx={{ width: 150 }}
										onChange={(e) => {
											setPageSize(parseInt(e.target.value, 10));
											setPage(1);
										}}>
										{[5, 10, 20, 50].map((n) => (
											<MenuItem key={n} value={n}>
												{n}
											</MenuItem>
										))}
									</Select>
								</FormControl>

								<Pagination
									count={Math.ceil(totalCount / pageSize)}
									page={page}
									onChange={(e, newPage) => setPage(newPage)}
									color="primary"
								/>
							</Box>
							{posts.length > 0 ? (
								posts.map((car) => (
									<Card
										variant="outlined"
										key={car.id}
										sx={{ marginY: "1rem" }}>
										<CarPost car={car} />
										<CardActions>
											<Button
												size="small"
												onClick={() => handleButtonClick(car.id)}>
												View Details
											</Button>
										</CardActions>
									</Card>
								))
							) : (
								<Container>
									<Card>
										<CardContent>
											<Typography variant="h5">
												No Cars Available Yet
											</Typography>
										</CardContent>
									</Card>
								</Container>
							)}
						</Container>
					</Container>
				</header>
			</div>
		</Box>
	);
}
