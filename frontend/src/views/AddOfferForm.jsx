import { useState, useEffect } from "react";
import {
	FormButton,
	FormCard,
	FormContainer,
	FormTitle,
} from "../components/StyledComponents";
import {
	Box,
	Alert,
	AlertTitle,
	Avatar,
	Button,
	Typography,
} from "@mui/material";
import axiosClient from "../axiosClient";
import { useSearchParams } from "react-router";

const AddOfferForm = () => {
	const { postId } = useSearchParams();
	const [err, setErr] = useState(null);
	const [msg, setMsg] = useState(null);
	const [isLoading, setIsLoading] = useState(false);
	const [licenseFile, setLicenseFile] = useState(null);
	const [proposalFile, setProposalFile] = useState(null);
	const [licensePreview, setLicensePreview] = useState();

	useEffect(() => {
		if (!licenseFile) {
			setLicensePreview(undefined);
			return;
		}
		const url = URL.createObjectURL(licenseFile);
		setLicensePreview(url);
		return () => URL.revokeObjectURL(url);
	}, [licenseFile]);

	const handleSubmit = async (e) => {
		e.preventDefault();
		setErr("");
		setMsg("");
		setIsLoading(true);

		const formData = new FormData();
		formData.append("LicenseFile", licenseFile);
		formData.append("ProposalFile", proposalFile);

		try {
			await axiosClient.post(`/Rental/Apply?CarId=${postId}`, formData, {
				withCredentials: true,
			});
			setMsg("Offer Applied successfully");
		} catch (err) {
			setErr(err.message);
			console.log(err);
		} finally {
			setIsLoading(false);
		}
	};

	return (
		<FormContainer
			sx={{
				display: "flex",
				flexDirection: "column",
				minHeight: "100vh",
			}}>
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
						<FormTitle variant="h5">Apply for Car Rental</FormTitle>

						{/* License upload with preview */}
						<Box
							sx={{
								display: "flex",
								flexDirection: "column",
								alignItems: "center",
							}}>
							<Avatar
								variant="rounded"
								sx={{ width: 100, height: 100, mb: 1 }}
								src={licensePreview}
							/>
							<Button variant="outlined" component="label">
								Upload License
								<input
									type="file"
									accept="image/*,application/pdf"
									hidden
									onChange={(e) => setLicenseFile(e.target.files?.[0] || null)}
								/>
							</Button>
							{licenseFile && (
								<Typography size="body2">{licenseFile.name}</Typography>
							)}
						</Box>
						{/* Proposal upload */}
						<Button variant="outlined" component="label">
							Upload Proposal
							<input
								type="file"
								accept="application/pdf,application/msword"
								hidden
								onChange={(e) => setProposalFile(e.target.files?.[0] || null)}
							/>
						</Button>
						{proposalFile && (
							<Typography size="body2">{proposalFile.name}</Typography>
						)}

						<FormButton
							type="submit"
							variant="contained"
							disabled={!licenseFile || !proposalFile || isLoading}>
							Submit Application
						</FormButton>
					</Box>
				</form>
			</FormCard>
		</FormContainer>
	);
};

export default AddOfferForm;
