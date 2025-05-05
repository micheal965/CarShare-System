/* eslint-disable react/prop-types */
import { Box, Typography } from "@mui/material";

const Notification = ({ notification }) => {
	return (
		<Box
			sx={{
				width: "100%",
				whiteSpace: "pre-wrap",
				overflowWrap: "break-word",
				wordBreak: "break-word",
				"& a": {
					color: "primary.main",
					textDecoration: "underline",
					"&:hover": {
						color: "primary.dark",
					},
				},
			}}>
			{notification.split("\n").map((line, index) => (
				<Typography
					key={index}
					variant="body1"
					component="div"
					sx={{
						lineHeight: 1.5,
						...(index > 0 && { mt: 1 }),
					}}>
					{line.startsWith("http") ? (
						<a href={line} target="_blank" rel="noopener noreferrer">
							{line}
						</a>
					) : (
						line
					)}
				</Typography>
			))}
		</Box>
	);
};

export default Notification;
