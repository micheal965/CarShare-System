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
		carOwner: "",
		title: "",
		brand: "",
		model: "",
		year: "",
		description: "",
		carType: "",
		transmission: "",
		location: "",
		rentalStatus: "",
		availableFrom: "",
		availableTo: "",
		price: "",
		image: null,
	});

	const [fetching, setFetching] = useState(true);
	const [saving, setSaving] = useState(false);
	const [err, setErr] = useState(null);
	const [msg, setMsg] = useState(null);

	useEffect(() => {
		axiosClient
			.get(`/Post/Get-Post/${id}`)
			.then(({ data }) => {
				setFormData({
					carOwner: data.carOwner,
					title: data.Title,
					brand: data.Brand,
					model: data.Model,
					year: data.Year.toString(),
					description: data.Description,
					carType: data.CarType,
					transmission: data.Transmission,
					location: data.Location,
					rentalStatus: data.RentalStatus,
					availableFrom: data.AvailabilityStart,
					availableTo: data.AvailabilityEnd,
					price: data.RentalPrice.toString(),
					images: data.ImageUrls,
				});
			})
			.catch(() => setErr("Failed to load car data."))
			.finally(() => setFetching(false));
	}, [id]);

	const handleChange = (e) => {
		const { name, value } = e.target;
		setFormData((f) => ({ ...f, [name]: value }));
	};
	const handleFileChange = (e) => {
		const file = e.target.files[0];
		setFormData((f) => ({ ...f, image: file }));
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
			const { data } = await axiosClient.put(`/cars/${id}`, payload);
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
							name="carOwner"
							label="Car Owner"
							variant="standard"
							value={formData.carOwner}
							onChange={handleChange}
							required
						/>
						<FormTextField
							name="title"
							label="Title"
							variant="standard"
							value={formData.title}
							onChange={handleChange}
							required
						/>
						<FormTextField
							name="brand"
							label="Brand"
							variant="standard"
							value={formData.brand}
							onChange={handleChange}
							required
						/>
						<FormTextField
							name="model"
							label="Model"
							variant="standard"
							value={formData.model}
							onChange={handleChange}
							required
						/>
						<FormTextField
							name="year"
							label="Year"
							type="number"
							variant="standard"
							value={formData.year}
							onChange={handleChange}
							required
						/>

						<FormTextField
							name="description"
							label="Description"
							variant="standard"
							multiline
							rows={3}
							value={formData.description}
							onChange={handleChange}
						/>

						<FormTextField
							name="carType"
							label="Car Type"
							variant="standard"
							value={formData.carType}
							onChange={handleChange}
						/>

						<FormControl variant="standard" fullWidth>
							<InputLabel>Transmission</InputLabel>
							<Select
								name="transmission"
								value={formData.transmission}
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
							name="location"
							label="Location"
							variant="standard"
							value={formData.location}
							onChange={handleChange}
						/>

						<FormTextField
							name="availableFrom"
							label="Available From"
							type="date"
							variant="standard"
							InputLabelProps={{ shrink: true }}
							value={formData.availableFrom}
							onChange={handleChange}
						/>
						<FormTextField
							name="availableTo"
							label="Available To"
							type="date"
							variant="standard"
							InputLabelProps={{ shrink: true }}
							value={formData.availableTo}
							onChange={handleChange}
						/>

						<FormTextField
							name="price"
							label="Price per Day"
							type="number"
							variant="standard"
							value={formData.price}
							onChange={handleChange}
							required
						/>

						<FormButton variant="outlined" component="label">
							{formData.images && !(formData.images instanceof File)
								? "Change Image"
								: "Upload Image"
								}
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
