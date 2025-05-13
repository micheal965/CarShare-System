import { Navigate, Outlet } from "react-router-dom";
import { useStateContext } from "../context/contextprovider";

import { Box, Container } from "@mui/material";
import MiniDrawer from "../components/Drawer";

export default function DefaultLayout() {
  const { token } = useStateContext();
  if (!token) {
    return <Navigate to="/login" />;
  }
  return (
    <Box id="defaultLayout" sx={{ display: "block" }}>
      <div className="content">
        <header>
          <MiniDrawer></MiniDrawer>
          <Container sx={{ mt: 10, pl: { sm: 10, xs: 6 }, pr: { xs: 0 } }}>
            <Outlet />
          </Container>
        </header>
      </div>
    </Box>
  );
}
