/* eslint-disable react/prop-types */
import CardMedia from "@mui/material/CardMedia";
import CardContent from "@mui/material/CardContent";
import { Typography } from "@mui/material";

export const CarPost = ({ car }) => {
	return (
		<>
			<CardMedia
				component="img"
				height="150"
				image={car.imageUrls[0] ? car.imageUrls[0] : null}
				alt={`${car.brand} ${car.model}`}
				sx={{ objectFit: "contain", width: "auto" }}
			/>

			<CardContent>
				<Typography variant="h6" color="text.secondary" gutterBottom>
					{car.title} - {car.brand} {car.model} ({car.year})
				</Typography>
				<Typography variant="body2" sx={{ marginBottom: 1 }}>
					{car.description}
				</Typography>
				<Typography variant="body2">
					<strong>Type:</strong> {car.carType} | <strong>Transmission:</strong>{" "}
					{car.transmission} <br />
					<strong>Location:</strong> {car.location} <br />
					<strong>Status:</strong> {car.rentalStatus} <br />
					<strong>Available From:</strong> {car.availabilityStart.split("T")[0]}{" "}
					to {car.availabilityEnd.split("T")[0]}
					<br />
					<strong>Price:</strong> ${car.rentalPrice}/day
				</Typography>
			</CardContent>
		</>
	);
};
