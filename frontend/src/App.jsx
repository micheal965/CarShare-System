import { RouterProvider } from "react-router-dom";
import router from "./router";
import { Container } from "@mui/material";
function App() {
  return (
    <Container sx={{ display: "block", bgcolor: "primary.main" }}>
      <RouterProvider router={router} />
    </Container>
  );
}

export default App;