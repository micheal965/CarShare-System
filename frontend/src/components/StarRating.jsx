import { Stack } from "@mui/material";
import { Star, StarBorder } from "@mui/icons-material";
import PropTypes from "prop-types";

const StarRating = ({ rating }) => {
	return (
		<Stack direction="row">
			{[...Array(5)].map((_, index) =>
				index < rating ? (
					<Star key={index} sx={{ color: "#ffc107" }} />
				) : (
					<StarBorder key={index} sx={{ color: "#e0e0e0" }} />
				)
			)}
		</Stack>
	);
};

StarRating.propTypes = {
	rating: PropTypes.number.isRequired,
};

export default StarRating;
