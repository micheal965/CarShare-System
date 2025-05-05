import {
	Avatar,
	Card,
	CardContent,
	Container,
	Skeleton,
	Typography,
	Alert,
	AlertTitle,
	Box,
} from "@mui/material";
import { useEffect, useState } from "react";
import axiosClient from "./../axiosClient";
import { FormButton } from "../components/StyledComponents";
import { convertImage } from "./../utils/convertImage";
import { useSearchParams } from "react-router";

const Proposals = () => {
	const { postId } = useSearchParams();

	const [proposals, setProposals] = useState([]);
	const [loading, setLoading] = useState(true);

	const [buttonLoading, setButtonLoading] = useState(false);
	const [msg, setMsg] = useState(null);
	const [err, setErr] = useState(null);

	const handleApprove = async (proposalId) => {
		setButtonLoading(true);
		try {
			await axiosClient.post(
				`/Offer/Accept/${proposalId}`,
				{},
				{ withCredentials: true }
			);
			setProposals((prev) => prev.filter((p) => p.id !== proposalId));
			setMsg("Proposal acceptted successfully");
		} catch (e) {
			setErr(e.response?.data?.message || "Approval failed.");
		} finally {
			setButtonLoading(false);
		}
	};

	const handleReject = async (proposalId) => {
		setButtonLoading(true);
		try {
			await axiosClient.post(
				`/Offer/Refuse/${proposalId}`,
				{},
				{ withCredentials: true }
			);
			setProposals((prev) => prev.filter((p) => p.id !== proposalId));
			setMsg("Proposal rejected successfully");
		} catch (e) {
			setErr(e.response?.data?.message || "Rejection failed.");
		} finally {
			setButtonLoading(false);
		}
	};

	useEffect(() => {
		axiosClient
			.get(`/Offer/Get-Offers/${postId}`, { withCredentials: true })
			.then(({ data }) => {
				setProposals(data);
				console.log(data);
			})
			.catch(() => {
				setErr("Failed to load proposals.");
			})
			.finally(() => {
				setLoading(false);
			});
	}, [postId]);

	if (loading) {
		return (
			<Container sx={{ my: 2 }}>
				{[1, 2, 3].map((i) => (
					<Skeleton key={i} variant="rectangular" height={200} sx={{ my: 1 }} />
				))}
			</Container>
		);
	}

	return (
		<Container sx={{ my: 2 }}>
			<Typography variant="h5" gutterBottom>
				Pending Rental Proposals
			</Typography>

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

			{proposals.length > 0 ? (
				proposals.map((p) => (
					<Card key={p.id} variant="outlined" sx={{ my: 2 }}>
						<CardContent>
							<Box
								sx={{ display: "flex", alignItems: "center", mb: 2, gap: 2 }}>
								<Avatar
									src={
										p.renter.avatar ? convertImage(p.renter.avatar.data) : null
									}
									alt={`${p.renter.first_name} ${p.renter.last_name}`}
									sx={{ width: 64, height: 64, objectFit: "cover" }}
								/>
								<Box>
									<Typography variant="h6">
										{p.renter.first_name} {p.renter.last_name}
									</Typography>
									<Typography color="text.secondary">
										{p.renter.email}
									</Typography>
								</Box>
							</Box>

							<Typography gutterBottom>
								<strong>Car:</strong> {p.car.brand} {p.car.model} ({p.car.year})
							</Typography>
							<Typography gutterBottom>
								<strong>Requested Dates:</strong> {p.availableFrom} –{" "}
								{p.availableTo}
							</Typography>
							<Typography gutterBottom>
								<strong>Proposed Price:</strong> ${p.proposedPrice}/day
							</Typography>
							<Typography gutterBottom>
								<strong>Message:</strong> {p.message || "—"}
							</Typography>

							<Box sx={{ display: "flex", gap: 1, mt: 2 }}>
								<FormButton
									variant="contained"
									color="success"
									onClick={() => handleApprove(p.id)}
									loading={buttonLoading}>
									Approve
								</FormButton>
								<FormButton
									variant="outlined"
									color="error"
									onClick={() => handleReject(p.id)}>
									Reject
								</FormButton>
							</Box>
						</CardContent>
					</Card>
				))
			) : (
				<Typography variant="h6" align="center" sx={{ mt: 4 }}>
					No pending proposals.
				</Typography>
			)}
		</Container>
	);
};

export default Proposals;
