import { useRef, useState } from "react";
import { Link } from "react-router-dom";
import {
	FormButton,
	FormCard,
	FormContainer,
	FormTextField,
	FormTitle,
} from "../components/StyledComponents";
import axiosClient from "../axiosClient";
import { useStateContext } from "../context/contextprovider";
import { Typography, Alert, AlertTitle } from "@mui/material";
import DarkModeButton from "./../components/darkmodeButton";
import {useNavigate} from "react-router"

export default function Login() {
	const emailRef = useRef();
	const passwordRef = useRef();
	const { setUser, setToken } = useStateContext();
	const [err, setErr] = useState(null);
	const [isLoading, setIsLoading] = useState(false);
	const navigate = useNavigate()


	const submit = (ev) => {
		ev.preventDefault();
		setIsLoading(true);
		setErr("");
		const payload = {
			Email: emailRef.current.value,
			Password: passwordRef.current.value,
		};

		axiosClient
			.post("/Auth/Login", payload)
			.then(({ data }) => {
				const userData = {
					id: data.id,
					username: data.username,
					email: data.email,
					userRole: data.roles[0],
					profilePic: data.profilePicture,
				};

				setUser(userData);
				setToken(data.token);
				navigate("/home")
			})
			.catch((err) => {
				const response = err.response;
				if (response) {
					setErr(
						response.data?.errors
							? response.data?.errors[0]
							: response.data?.message
					);
				}
			})
			.finally(() => {
				setIsLoading(false);
			});
	};

	return (
		<FormContainer sx={{ display: "flex", flexDirection: "column" }}>
			<FormCard>
				{err && (
					<Alert severity="error" sx={{ marginBottom: "1rem" }}>
						<AlertTitle>Error</AlertTitle>
						{err}
					</Alert>
				)}

				<FormTitle className="title">Login</FormTitle>
				<form onSubmit={submit}>
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
					<FormButton
						type="submit"
						variant="contained"
						className="btn btn-black btn-block"
						loading={isLoading}>
						Login
					</FormButton>
					<br />
					<Typography
						variant="body2"
						sx={{ marginTop: "1rem", textAlign: "center" }}
						className="message">
						Not Registered? <Link to="/register">Create a new account</Link>
					</Typography>
				</form>
			</FormCard>
			<DarkModeButton></DarkModeButton>
		</FormContainer>
	);
}
