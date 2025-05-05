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
				image={car.ImageUrls[0]}
				alt={`${car.Brand} ${car.Model}`}
				sx={{ objectFit: "contain", width: "auto" }}
			/>

			<CardContent>
				<Typography variant="h6" color="text.secondary" gutterBottom>
					{car.Title} - {car.Brand} {car.Model} ({car.Year})
				</Typography>
				<Typography variant="body2" sx={{ marginBottom: 1 }}>
					{car.Description}
				</Typography>
				<Typography variant="body2">
					<strong>Type:</strong> {car.CarType} | <strong>Transmission:</strong>{" "}
					{car.Transmission} <br />
					<strong>Location:</strong> {car.Location} <br />
					<strong>Status:</strong> {car.RentalStatus} <br />
					<strong>Available From:</strong> {car.AvailabilityStart} to{" "}
					{car.AvailabilityEnd}
					<br />
					<strong>Price:</strong> ${car.RentalPrice}/day
				</Typography>
			</CardContent>
		</>
	);
};
