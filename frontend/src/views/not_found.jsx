import { Container, Typography, Button, Box } from "@mui/material";
import { Link as RouterLink } from "react-router-dom"; // If using React Router

function NotFoundPage() {
  return (
    <Container
      sx={{
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
        minHeight: "100vh",
        textAlign: "center",
      }}
    >
      <Box sx={{ mb: 4 }}>
        <Typography variant="h1" sx={{ fontSize: "6rem", fontWeight: "bold" }}>
          404
        </Typography>
        <Typography variant="h4" sx={{ mb: 2 }}>
          Page Not Found
        </Typography>
        <Typography variant="body1" sx={{ mb: 4 }}>
          Oops! The page you are looking for does not exist.
        </Typography>
      </Box>

      <Box sx={{ display: "flex", gap: 2 }}>
        <Button variant="contained" component={RouterLink} to="/">
          Go to Home
        </Button>
      </Box>
    </Container>
  );
}

export default NotFoundPage;
