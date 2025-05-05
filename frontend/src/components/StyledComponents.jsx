/* eslint-disable no-unused-vars */
import {
	Button,
	Card,
	Container,
	styled,
	TextField,
	Typography,
	Checkbox,
	FormControlLabel,
} from "@mui/material";

export const FormCheckbox = styled(Checkbox)({
	color: "inherit",
	"&.Mui-checked": {
		color: "inherit",
	},
});

export const FormCheckboxLabel = styled(FormControlLabel)({
	marginTop: "1rem",
	marginBottom: "1rem",
	width: "100%",
	display: "flex",
	alignItems: "center",
	".MuiFormControlLabel-label": {
		fontSize: "1rem",
	},
});

export const FormTextField = styled(TextField)(({ theme }) => ({
	width: "100%",
	marginBottom: "1rem",
}));

export const FormButton = styled(Button)(({ theme }) => ({
	width: "100%",
}));

export const FormTitle = styled(Typography)(({ theme }) => ({
	fontSize: "1.5rem",
	fontWeight: "bold",

	marginBottom: ".5rem",
	textAlign: "center",
}));

export const FormCard = styled(Card)(({ variant, theme }) => ({
	width: variant === "register" ? "720px" : "360px",
	position: "relative",
	zIndex: 1,

	padding: "34px",
}));

export const FormContainer = styled(Container)(({ theme }) => ({
	minHeight: "100vh",

	display: "flex",
	justifyContent: "center",
	alignItems: "center",
}));
