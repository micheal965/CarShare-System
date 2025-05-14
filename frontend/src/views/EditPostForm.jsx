import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
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
	Skeleton,
	Container,
} from "@mui/material";
import axiosClient from "../axiosClient";

export const EditPostForm = () => {
	const { id } = useParams();
	const navigate = useNavigate();

	const [formData, setFormData] = useState({
		Id: id,
		Title: "",
		Brand: "",
		Model: "",
		Year: "",
		Description: "",
		CarType: "",
		Transmission: "",
		Location: "",
		RentalStatus: "",
		AvailabilityStart: "",
		AvailabilityEnd: "",
		RentalPrice: "",
		Images: null,
	});

	const [fetching, setFetching] = useState(true);
	const [saving, setSaving] = useState(false);
	const [err, setErr] = useState(null);
	const [msg, setMsg] = useState(null);

	useEffect(() => {
		const fetchPostData = async () => {
			await axiosClient
				.get(`/Post/Get-Post/${id}`)
				.then(({ data }) => {
					console.log(data);
					setFormData({
						Id: id,
						Title: data.title,
						Brand: data.brand,
						Model: data.model,
						Year: data.year.toString(),
						Description: data.description,
						CarType: data.carType,
						Transmission: data.transmission,
						Location: data.location,
						RentalStatus: data.rentalStatus,
						AvailabilityStart: data.availabilityStart.split("T")[0],
						AvailabilityEnd: data.availabilityEnd.split("T")[0],
						RentalPrice: data.rentalPrice.toString(),
						Images: data.imageUrls,
					});
				})
				.catch((err) => console.log(err))
				.finally(() => setFetching(false));
		};

		console.log(id)
		fetchPostData();
	}, [id]);

	const handleChange = (e) => {
		const { name, value } = e.target;
		setFormData((f) => ({ ...f, [name]: value }));
	};
	const handleFileChange = (e) => {
		const file = e.target.files[0];
		setFormData((f) => ({ ...f, Images: file }));
	};

	const handleSubmit = async (e) => {
		e.preventDefault();
		setSaving(true);
		setErr(null);
		setMsg(null);

		const payload = new FormData();
		Object.entries(formData).forEach(([key, val]) => {
			if (key === "image" && val instanceof File) {
				payload.append(key, val);
			} else {
				payload.append(key, val);
			}
		});


		try {
			const { data } = await axiosClient.put("/Post/Update-Post", payload, {
				headers: { "Content-Type": "multipart/form-data" },
				withCredentials: true,
			});
			setMsg(data.message || "Updated successfully!");
			navigate(`/car`, {
				state: {
					carId: id,
				},
			});
		} catch (e) {
			setErr(e.response?.data?.message || "Update failed.");
		} finally {
			setSaving(false);
		}
	};

	if (fetching) {
		return (
			<Container>
				<Skeleton variant="rectangle" height={500} />
			</Container>
		);
	}

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

				<form onSubmit={handleSubmit}>
					<Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
						<FormTitle variant="h5">Edit Car Post</FormTitle>

						<FormTextField
							name="Title"
							label="Title"
							variant="standard"
							value={formData.Title}
							onChange={handleChange}
							required
						/>
						<FormTextField
							name="Brand"
							label="Brand"
							variant="standard"
							value={formData.Brand}
							onChange={handleChange}
							required
						/>
						<FormTextField
							name="Model"
							label="Model"
							variant="standard"
							value={formData.Model}
							onChange={handleChange}
							required
						/>
						<FormTextField
							name="Year"
							label="Year"
							type="number"
							variant="standard"
							value={formData.Year}
							onChange={handleChange}
							required
						/>

						<FormTextField
							name="Description"
							label="Description"
							variant="standard"
							multiline
							rows={3}
							value={formData.Description}
							onChange={handleChange}
						/>

						<FormTextField
							name="CarType"
							label="Car Type"
							variant="standard"
							value={formData.CarType}
							onChange={handleChange}
						/>

						<FormControl variant="standard" fullWidth>
							<InputLabel>Transmission</InputLabel>
							<Select
								name="Transmission"
								value={formData.Transmission}
								onChange={handleChange}
								required>
								{["Automatic", "Manual"].map((t) => (
									<MenuItem key={t} value={t}>
										{t}
									</MenuItem>
								))}
							</Select>
						</FormControl>

						<FormTextField
							name="Location"
							label="Location"
							variant="standard"
							value={formData.Location}
							onChange={handleChange}
						/>

						<FormTextField
							name="AvailabilityStart"
							label="Available From"
							type="date"
							variant="standard"
							InputLabelProps={{ shrink: true }}
							value={formData.AvailabilityStart}
							onChange={handleChange}
						/>
						<FormTextField
							name="AvailabilityEnd"
							label="Available To"
							type="date"
							variant="standard"
							InputLabelProps={{ shrink: true }}
							value={formData.AvailabilityEnd}
							onChange={handleChange}
						/>

						<FormTextField
							name="RentalPrice"
							label="Price per Day"
							type="number"
							variant="standard"
							value={formData.RentalPrice}
							onChange={handleChange}
							required
						/>

						<FormButton variant="outlined" component="label">
							{formData.Images && !(formData.Images instanceof File)
								? "Change Image"
								: "Upload Image"}
							<input
								hidden
								accept="image/*"
								type="file"
								multiple
								onChange={handleFileChange}
							/>
						</FormButton>

						<FormButton variant="contained" type="submit" loading={saving}>
							Save Changes
						</FormButton>
					</Box>
				</form>
			</FormCard>
		</FormContainer>
	);
};
