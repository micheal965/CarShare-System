import { useRef, useState } from "react";
import {
	FormButton,
	FormCard,
	FormContainer,
	FormTextField,
	FormTitle,
	FormCheckbox,
	FormCheckboxLabel,
} from "../components/StyledComponents";
import { Link } from "react-router-dom";
import axiosClient from "../axiosClient";
import AlertTitle from "@mui/material/AlertTitle";
import { Alert, Typography, Box, Avatar } from "@mui/material";
import DarkModeButton from "./../components/darkmodeButton";

export default function Register() {
	const firstNameRef = useRef();
	const lastNameRef = useRef();
	const phoneNumberRef = useRef();
	const emailRef = useRef();
	const passwordRef = useRef();
	const confirmPasswordRef = useRef();

	const [err, setErr] = useState(null);
	const [msg, setMsg] = useState(null);
	const [isCarOwner, setIsCarOwner] = useState(false);
	const [imageFile, setImageFile] = useState(null);

	const [isButtonloading, setIsButtonLoading] = useState(false);

	const handleFileChange = (e) => {
		setImageFile(e.target.files[0] || null);
	};

	const submit = async (ev) => {
		ev.preventDefault();
		setIsButtonLoading(true);
		setErr("");
		if (passwordRef.current.value == confirmPasswordRef.current.value) {
			const formData = new FormData();
			formData.append("FirstName", firstNameRef.current.value);
			formData.append("LastName", lastNameRef.current.value);
			formData.append("PhoneNumber", phoneNumberRef.current.value);
			formData.append("Email", emailRef.current.value);
			formData.append("Password", passwordRef.current.value);
			formData.append("ConfirmPassword", passwordRef.current.value);

			formData.append("role", isCarOwner ? 0 : 1);
			formData.append("ProfilePicture", imageFile);

			axiosClient
				.post("/Auth/Register", formData)
				.then(() => {
					setMsg("Registered Successfully!");
				})
				.catch((err) => {
					const response = err.response;
					console.log(response.data.message);
					if (response) {
						setErr(response.data.message);
					}
				})
				.finally(() => setIsButtonLoading(false));
		} else {
			setErr("Passwords do not match.");
			setIsButtonLoading(false);
			console.log(err);
		}
	};

	return (
		<FormContainer
			sx={{ display: "flex", flexDirection: "column", minHeight: "100vh" }}>
			<FormCard className="form" sx={{ maxHeight: "200vh" }}>
				{err && (
					<Alert severity="error" sx={{ marginBottom: "1rem" }}>
						<AlertTitle>Error</AlertTitle>
						{err}
					</Alert>
				)}
				{msg && (
					<Alert severity="success" sx={{ marginBottom: "1rem" }}>
						<AlertTitle>Success</AlertTitle>
						{msg}
					</Alert>
				)}
				<form onSubmit={submit}>
					<FormTitle variant="h1" className="title">
						Personal Information
					</FormTitle>
					<Box
						sx={{
							marginY: "1rem",
							display: "flex",
							flexDirection: "column",
							alignItems: "center",
						}}>
						<Avatar
							sx={{ width: 100, height: 100, marginY: "1rem" }}
							src={imageFile ? URL.createObjectURL(imageFile) : null}
						/>
						<FormButton variant="outlined" component="label">
							Upload Image
							<input
								hidden
								accept="image/*"
								type="file"
								onChange={handleFileChange}
							/>
						</FormButton>
					</Box>
					<Box
						sx={{
							display: "flex",
							flexDirection: { xs: "column", sm: "row" },
							gap: "1rem",
						}}>
						<Box sx={{ width: "100%" }}>
							<FormTextField
								inputRef={firstNameRef}
								type="name"
								name=""
								id="standard-basic"
								label="First name"
								variant="standard"
								required
							/>
							<FormTextField
								inputRef={lastNameRef}
								type="name"
								name=""
								id="standard-basic"
								label="Last name"
								variant="standard"
								required
							/>
							<FormTextField
								inputRef={phoneNumberRef}
								type="name"
								name=""
								id="standard-basic"
								label="Phone number"
								variant="standard"
								required
							/>
							<FormTextField
								inputRef={emailRef}
								type="email"
								name=""
								id="standard-basic"
								label="Email"
								variant="standard"
								required
							/>
							<FormTextField
								inputRef={passwordRef}
								type="password"
								name=""
								id="standard-basic"
								label="Password"
								variant="standard"
								required
							/>
							<FormTextField
								inputRef={confirmPasswordRef}
								type="password"
								name=""
								id="standard-basic"
								label="Confirm Password"
								variant="standard"
								required
							/>
							<FormCheckboxLabel
								control={
									<FormCheckbox
										checked={isCarOwner}
										onChange={(e) => setIsCarOwner(e.target.checked)}
									/>
								}
								label="car owner"
							/>
						</Box>
					</Box>
					<FormButton
						variant="contained"
						className="btn btn-black"
						type="submit"
						loading={isButtonloading}>
						Sign-Up
					</FormButton>
				</form>
				<Typography
					variant="body2"
					className="message"
					sx={{ marginTop: "1rem", textAlign: "center" }}>
					Already Have An Account? <Link to="/login">Login</Link>
				</Typography>
			</FormCard>
			<DarkModeButton></DarkModeButton>
		</FormContainer>
	);
}
