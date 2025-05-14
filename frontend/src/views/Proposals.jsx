import {
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
import { useParams } from "react-router";
import Button from "@mui/material/Button";
import DescriptionIcon from "@mui/icons-material/Description";
import PictureAsPdfIcon from "@mui/icons-material/PictureAsPdf";

const Proposals = () => {
	const { postId } = useParams();

	const [proposals, setProposals] = useState([]);
	const [loading, setLoading] = useState(true);

	const [buttonLoading, setButtonLoading] = useState(false);
	const [msg, setMsg] = useState(null);
	const [err, setErr] = useState(null);

	const handleApprove = async (proposalId) => {
		setButtonLoading(true);
		try {
			await axiosClient.put(
				`/Rental/Review`,
				{
					applicationId: proposalId,
					isAccepted: true,
				},
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
			await axiosClient.put(
				`/Rental/Review`,
				{
					applicationId: proposalId,
					isAccepted: false,
				},
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
		const fetchData = async () => {
			await axiosClient
				.get(`/Rental/Get-All-Pending-RentalApps/${postId}`, {
					withCredentials: true,
				})
				.then(({ data }) => {
					console.log(data);
					setProposals(data);
				})
				.catch(() => setErr("Failed to load proposals."))
				.finally(() => setLoading(false));
		};

		fetchData();
	}, [postId]);

	const daysSinceApplication = (date) => {
		const applicationDate = new Date(date);
		const now = new Date();

		const MS_PER_DAY = 1000 * 60 * 60 * 24;

		return Math.floor((now - applicationDate) / MS_PER_DAY);
	};

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
							{/* New file links */}
							<Box
								sx={{
									display: "flex",
									flexDirection: "column",
									gap: 1,
									mt: 2,
								}}>
								{p.licenseFileUrl && (
									<Button
										component="a"
										href={p.licenseFileUrl}
										target="_blank"
										rel="noopener noreferrer"
										startIcon={<PictureAsPdfIcon />}
										variant="outlined"
										size="small">
										View License
									</Button>
								)}
								{p.proposalFileUrl && (
									<Button
										component="a"
										href={p.proposalFileUrl}
										target="_blank"
										rel="noopener noreferrer"
										startIcon={<DescriptionIcon />}
										variant="outlined"
										size="small">
										View Proposal
									</Button>
								)}

								<Box sx={{ mt: 1 }}>
									<Typography variant="body2" color="text.secondary">
										Rented from {daysSinceApplication(p.applicationDate)} day
										{daysSinceApplication(p.applicationDate) !== 1
											? "s"
											: ""}{" "}
										ago
									</Typography>
								</Box>
							</Box>

							{/* Approve/Reject buttons */}
							<Box sx={{ display: "flex", gap: 1, mt: 3 }}>
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
