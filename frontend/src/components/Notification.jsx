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
			<Typography>
				{notification.message}
			</Typography>
		</Box>
	);
};

export default Notification;
