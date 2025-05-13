import { useRef, useState, useEffect } from "react";
import {
	FormButton,
	FormCard,
	FormContainer,
	FormTextField,
	FormTitle,
} from "../components/StyledComponents";
import {
	Box,
	MenuItem,
	FormControl,
	InputLabel,
	Select,
	Alert,
	AlertTitle,
	Avatar,
} from "@mui/material";
import axiosClient from "../axiosClient";

export const AddPostForm = () => {
	const titleRef = useRef();
	const brandRef = useRef();
	const modelRef = useRef();
	const yearRef = useRef();
	const descriptionRef = useRef();
	const locationRef = useRef();
	const carTypeRef = useRef();
	const priceRef = useRef();

	const [err, setErr] = useState(null);
	const [msg, setMsg] = useState(null);
	const [isLoading, setIsLoading] = useState(false);
	const [transmission, setTransmission] = useState("");
	const [imageFile, setImageFile] = useState(null);
	const [previewUrl, setPreviewUrl] = useState(undefined);

	const handleFileChange = (e) => {
		const selected = e.target.files?.[0] ?? null;
		setImageFile(selected);
	};

	const submit = async (e) => {
		e.preventDefault();
		setIsLoading(true);
		setErr(null);
		setMsg(null);

		const formData = new FormData();
		formData.append("Title", titleRef.current.value);
		formData.append("Description", descriptionRef.current.value);
		formData.append("CarType", carTypeRef.current.value);
		formData.append("Brand", brandRef.current.value);
		formData.append("Model", modelRef.current.value);
		formData.append("Year", yearRef.current.value);
		formData.append("Transmission", transmission);
		formData.append("Location", locationRef.current.value);
		formData.append("RentalPrice", priceRef.current.value);
		formData.append("Images", imageFile);

		try {
			const { data } = await axiosClient.post("/Post/Create-Post", formData, {
				headers: { "Content-Type": "multipart/form-data" },
				withCredentials: true,
			});

			setMsg(data.message || "Post added successfully");
		} catch (err) {
			const response = err.response;
			if (response) {
				setErr(
					response.data?.errors
						? response.data?.errors[0]
						: response.data?.message
				);
			}
		} finally {
			setIsLoading(false);
		}
	};

	useEffect(() => {
		if (!imageFile) {
			setPreviewUrl(undefined);
			return;
		}

		const objectUrl = URL.createObjectURL(imageFile);
		setPreviewUrl(objectUrl);

		return () => {
			URL.revokeObjectURL(objectUrl);
		};
	}, [imageFile]);

	return (
		<FormContainer
			sx={{ display: "flex", flexDirection: "column", minHeight: "100vh" }}>
			<FormCard sx={{ maxWidth: 600, mx: "auto", my: 4 }}>
				{err && (
					<Alert severity="error" sx={{ mb: 2 }}>
						<AlertTitle>Error</AlertTitle>
						{err}
					</Alert>
				)}
				{msg && (
					<Alert severity="success" sx={{ mb: 2 }}>
						<AlertTitle>Success</AlertTitle>
						{msg}
					</Alert>
				)}

				<form onSubmit={submit}>
					<Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
						<FormTitle variant="h5">Create Car Post</FormTitle>

						<FormTextField
							inputRef={titleRef}
							label="Title"
							variant="standard"
							required
						/>
						<FormTextField
							inputRef={brandRef}
							label="Brand"
							variant="standard"
							required
						/>
						<FormTextField
							inputRef={modelRef}
							label="Model"
							variant="standard"
							required
						/>
						<FormTextField
							inputRef={yearRef}
							label="Year"
							type="number"
							variant="standard"
							required
						/>

						<FormTextField
							inputRef={descriptionRef}
							label="Description"
							variant="standard"
							multiline
							rows={3}
						/>

						<FormTextField
							inputRef={carTypeRef}
							label="Car Type"
							variant="standard"
						/>

						<FormControl variant="standard" fullWidth>
							<InputLabel>Transmission</InputLabel>
							<Select
								value={transmission}
								onChange={(e) => setTransmission(e.target.value)}
								label="Transmission"
								required>
								{["Automatic", "Manual"].map((t) => (
									<MenuItem key={t} value={t}>
										{t}
									</MenuItem>
								))}
							</Select>
						</FormControl>

						<FormTextField
							inputRef={locationRef}
							label="Location"
							variant="standard"
						/>

						<FormTextField
							inputRef={priceRef}
							label="Price per Day"
							type="number"
							variant="standard"
							required
						/>

						<Box
							sx={{
								marginY: "1rem",
								display: "flex",
								flexDirection: "column",
								alignItems: "center",
							}}>
							<Avatar
								sx={{ width: 100, height: 100, marginY: "1rem" }}
								src={previewUrl}
							/>
							<FormButton variant="outlined" component="label">
								Upload Image
								<input
									hidden
									accept="image/*"
									type="file"
									onChange={handleFileChange}
									multiple
								/>
							</FormButton>
						</Box>

						<FormButton variant="contained" type="submit" loading={isLoading}>
							Create Car Post
						</FormButton>
					</Box>
				</form>
			</FormCard>
		</FormContainer>
	);
};
