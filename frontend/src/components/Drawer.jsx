import * as React from "react";
import { styled, useTheme, alpha } from "@mui/material/styles";
import Box from "@mui/material/Box";
import MuiDrawer from "@mui/material/Drawer";
import MuiAppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import List from "@mui/material/List";
import CssBaseline from "@mui/material/CssBaseline";
import Divider from "@mui/material/Divider";
import IconButton from "@mui/material/IconButton";
import MenuIcon from "@mui/icons-material/Menu";
import ChevronLeftIcon from "@mui/icons-material/ChevronLeft";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemText from "@mui/material/ListItemText";
import MenuItem from "@mui/material/MenuItem";
import Menu from "@mui/material/Menu";
import SearchIcon from "@mui/icons-material/Search";
import AccountCircle from "@mui/icons-material/AccountCircle";
import MoreIcon from "@mui/icons-material/MoreVert";
import { useStateContext } from "../context/contextprovider";
import DarkModeButton from "./darkmodeButton";
import { useNavigate } from "react-router-dom";
import { TextField, Link, Avatar, Button } from "@mui/material";
import HomeIcon from "@mui/icons-material/Home";
import DirectionsCarIcon from "@mui/icons-material/DirectionsCar";
import AdminPanelSettingsIcon from "@mui/icons-material/AdminPanelSettings";
import {FormButton} from "./StyledComponents"

const drawerWidth = 240;

const openedMixin = (theme) => ({
	width: drawerWidth,
	transition: theme.transitions.create("width", {
		easing: theme.transitions.easing.sharp,
		duration: theme.transitions.duration.enteringScreen,
	}),
	overflowX: "hidden",
});

const closedMixin = (theme) => ({
	transition: theme.transitions.create("width", {
		easing: theme.transitions.easing.sharp,
		duration: theme.transitions.duration.leavingScreen,
	}),
	overflowX: "hidden",
	width: `calc(${theme.spacing(7)} + 1px)`,
	[theme.breakpoints.up("sm")]: {
		width: `calc(${theme.spacing(8)} + 1px)`,
	},
});

const DrawerHeader = styled("div")(({ theme }) => ({
	display: "flex",
	alignItems: "center",
	justifyContent: "flex-end",
	padding: theme.spacing(0, 1),

	...theme.mixins.toolbar,
}));

const AppBar = styled(MuiAppBar, {
	shouldForwardProp: (prop) => prop !== "open",
})(({ theme }) => ({
	zIndex: theme.zIndex.drawer + 1,
	transition: theme.transitions.create(["width", "margin"], {
		easing: theme.transitions.easing.sharp,
		duration: theme.transitions.duration.leavingScreen,
	}),
	variants: [
		{
			props: ({ open }) => open,
			style: {
				marginLeft: drawerWidth,
				width: `calc(100% - ${drawerWidth}px)`,
				transition: theme.transitions.create(["width", "margin"], {
					easing: theme.transitions.easing.sharp,
					duration: theme.transitions.duration.enteringScreen,
				}),
			},
		},
	],
}));

const Drawer = styled(MuiDrawer, {
	shouldForwardProp: (prop) => prop !== "open",
})(({ theme }) => ({
	width: drawerWidth,
	flexShrink: 0,
	whiteSpace: "nowrap",
	boxSizing: "border-box",
	variants: [
		{
			props: ({ open }) => open,
			style: {
				...openedMixin(theme),
				"& .MuiDrawer-paper": openedMixin(theme),
			},
		},
		{
			props: ({ open }) => !open,
			style: {
				...closedMixin(theme),
				"& .MuiDrawer-paper": closedMixin(theme),
			},
		},
	],
}));

const Search = styled("div")(({ theme }) => ({
	position: "relative",
	borderRadius: theme.shape.borderRadius,
	backgroundColor: alpha(theme.palette.common.white, 0.15),
	"&:hover": {
		backgroundColor: alpha(theme.palette.common.white, 0.25),
	},
	marginRight: theme.spacing(2),
	marginLeft: 0,
	width: "100%",
	[theme.breakpoints.up("sm")]: {
		marginLeft: theme.spacing(3),
		width: "auto",
	},
}));

const SearchIconWrapper = styled("div")(({ theme }) => ({
	padding: theme.spacing(0, 2),
	height: "100%",
	position: "absolute",
	pointerEvents: "none",
	display: "flex",
	alignItems: "center",
	justifyContent: "center",
}));

const StyledInputBase = styled(TextField)(({ theme }) => ({
	width: "100%",
	maxWidth: "500rem",
	marginLeft: theme.spacing(4),

	"& .MuiInputBase-root": {
		width: "90%",
		backgroundColor: "transparent",
		border: "none",
		boxShadow: "none",
	},

	"& .MuiOutlinedInput-notchedOutline": {
		border: "none",
	},

	"& .MuiInputBase-input": {
		width: "100%",
		padding: theme.spacing(1.2, 2),
		fontSize: "1rem",
		[theme.breakpoints.down("sm")]: {
			fontSize: "0.9rem",
			padding: theme.spacing(1),
		},
	},
}));

export default function MiniDrawer() {
	const theme = useTheme();
	const navigate = useNavigate();
	const [open, setOpen] = React.useState(false);

	const handleDrawerOpen = () => {
		setOpen(true);
	};

	const handleDrawerClose = () => {
		setOpen(false);
	};

	const [anchorEl, setAnchorEl] = React.useState(null);
	const [mobileMoreAnchorEl, setMobileMoreAnchorEl] = React.useState(null);

	const isMenuOpen = Boolean(anchorEl);
	const isMobileMenuOpen = Boolean(mobileMoreAnchorEl);

	const handleProfileMenuOpen = (event) => {
		setAnchorEl(event.currentTarget);
	};

	const handleMobileMenuClose = () => {
		setMobileMoreAnchorEl(null);
	};

	const handleMenuClose = () => {
		setAnchorEl(null);
		handleMobileMenuClose();
	};

	const handleMobileMenuOpen = (event) => {
		setMobileMoreAnchorEl(event.currentTarget);
	};
	const { setUser, setToken, user, token, logout } = useStateContext();

	const handlelogout = (ev) => {
		ev.preventDefault();
		setUser(null);
		setToken(null);
		logout();
	};

	const menuId = "primary-search-account-menu";
	const renderMenu = (
		<Menu
			anchorEl={anchorEl}
			anchorOrigin={{
				vertical: "top",
				horizontal: "right",
			}}
			id={menuId}
			keepMounted
			transformOrigin={{
				vertical: "top",
				horizontal: "right",
			}}
			open={isMenuOpen}
			onClose={handleMenuClose}>
			{token ? (
				<MenuItem onClick={handlelogout}>Log Out</MenuItem>
			) : (
				<>
					<MenuItem onClick={() => navigate("/login")}>Login</MenuItem>
					<MenuItem onClick={() => navigate("/register")}>Register</MenuItem>
				</>
			)}
		</Menu>
	);

	const mobileMenuId = "primary-search-account-menu-mobile";

	const handleSearchSubmit = (ev) => {
		ev.preventDefault();
		const raw = ev.target.elements["custom-search"].value.trim();
		if (!raw) return;

		// 2) Build your API call
		const params = new URLSearchParams();
		if (!isNaN(Number(raw))) {
			params.set("RentalPrice", raw);
		} else {
			params.set("CarType", raw);
		}

		const API_CALL = `/Post/Search-For-Cars?${params.toString()}`;
		navigate("/search-result", { state: { query: API_CALL } });
	};

	const renderMobileMenu = (
		<Menu
			anchorEl={mobileMoreAnchorEl}
			anchorOrigin={{
				vertical: "top",
				horizontal: "right",
			}}
			id={mobileMenuId}
			keepMounted
			transformOrigin={{
				vertical: "top",
				horizontal: "right",
			}}
			open={isMobileMenuOpen}
			onClose={handleMobileMenuClose}>
			<MenuItem onClick={handleProfileMenuOpen}>
				<IconButton
					size="large"
					edge="end"
					aria-label="account of current user"
					aria-controls={menuId}
					aria-haspopup="true"
					onClick={handleProfileMenuOpen}
					color="inherit">
					{user?.profilePic !== undefined ? (
						<Avatar src={user?.profilePic} />
					) : (
						<AccountCircle />
					)}
				</IconButton>
				<p>Profile</p>
			</MenuItem>
			{token === null && (
				<>
					<MenuItem>
						<Button onClick={() => navigate("/login")}>login</Button>
					</MenuItem>
					<MenuItem>
						<Button onClick={() => navigate("/register")}>Register</Button>
					</MenuItem>
				</>
			)}
			<MenuItem>
				<DarkModeButton></DarkModeButton>
			</MenuItem>
		</Menu>
	);

	return (
		<Box sx={{ display: "flex" }}>
			<CssBaseline />
			<AppBar position="fixed" open={open}>
				<Toolbar>
					<IconButton
						color="inherit"
						aria-label="open drawer"
						onClick={handleDrawerOpen}
						edge="start"
						sx={[
							{
								marginRight: 5,
							},
							open && { display: "none" },
						]}>
						<MenuIcon />
					</IconButton>

					<Link
						href="/home"
						variant="h6"
						noWrap
						sx={{ display: { xs: "none", sm: "block", fontWeight: "bold" } }}
						underline="none"
						color="inherit">
						Car Share
					</Link>
					<form
						onSubmit={handleSearchSubmit}
						style={{ display: "flex", flexGrow: 1 }}>
						<Search sx={{ flexGrow: 1 }}>
							<SearchIconWrapper>
								<SearchIcon />
							</SearchIconWrapper>
							<StyledInputBase
								id="custom-search"
								name="custom-search"
								placeholder="Search…"
								inputProps={{ type: "search" }}
								disabled={!token}
								// remove onChange here
							/>
						</Search>
						{/* you can submit via Enter or add a button */}
						<FormButton type="submit" disabled={!token} sx={{width: 20}}>
							Go
						</FormButton>
					</form>
					<Box sx={{ flexGrow: 1 }} />
					<Box sx={{ display: { xs: "none", md: "flex" } }}>
						<Box sx={{ padding: 1 }}>
							<DarkModeButton></DarkModeButton>
						</Box>

						<IconButton
							size="large"
							edge="end"
							aria-label="account of current user"
							aria-controls={menuId}
							aria-haspopup="true"
							onClick={handleProfileMenuOpen}
							color="inherit">
							{user?.profilePic !== undefined ? (
								<Avatar src={user?.profilePic} />
							) : (
								<AccountCircle />
							)}
						</IconButton>
					</Box>
					<Box sx={{ display: { xs: "flex", md: "none" } }}>
						<IconButton
							size="large"
							aria-label="show more"
							aria-controls={mobileMenuId}
							aria-haspopup="true"
							onClick={handleMobileMenuOpen}
							color="inherit">
							<MoreIcon />
						</IconButton>
					</Box>
				</Toolbar>
			</AppBar>
			<Drawer variant="permanent" open={open}>
				<DrawerHeader>
					<IconButton onClick={handleDrawerClose}>
						{theme.direction === "rtl" ? (
							<ChevronRightIcon />
						) : (
							<ChevronLeftIcon />
						)}
					</IconButton>
				</DrawerHeader>
				<Divider />
				<List>
					<ListItem
						key={"home"}
						component="a"
						href="/home"
						disablePadding
						sx={{
							display: "block",
							textDecoration: "none",
							color: theme.palette.text.primary,
							"&:visited": {
								color: theme.palette.text.primary,
							},
							"&:hover": {
								textDecoration: "none",
							},
						}}>
						<ListItemButton
							sx={[
								{
									minHeight: 48,
									px: 2.5,
								},
								open
									? {
											justifyContent: "initial",
										}
									: {
											justifyContent: "center",
										},
							]}>
							<ListItemIcon
								sx={[
									{
										minWidth: 0,
										justifyContent: "center",
									},
									open
										? {
												mr: 3,
											}
										: {
												mr: "auto",
											},
								]}>
								<HomeIcon />
							</ListItemIcon>
							<ListItemText
								primary={"Home"}
								sx={[
									open
										? {
												opacity: 1,
											}
										: {
												opacity: 0,
											},
								]}
							/>
						</ListItemButton>
					</ListItem>
					{
						<ListItem
							key={"Posts"}
							component="a"
							href="/posts"
							disablePadding
							sx={{
								display: user?.userRole === "CarOwner" ? "block" : "none",
								textDecoration: "none",
								color: theme.palette.text.primary,
								"&:visited": {
									color: theme.palette.text.primary,
								},
								"&:hover": {
									textDecoration: "none",
								},
							}}>
							<ListItemButton
								sx={[
									{
										minHeight: 48,
										px: 2.5,
									},
									open
										? {
												justifyContent: "initial",
											}
										: {
												justifyContent: "center",
											},
								]}>
								<ListItemIcon
									sx={[
										{
											minWidth: 0,
											justifyContent: "center",
										},
										open
											? {
													mr: 3,
												}
											: {
													mr: "auto",
												},
									]}>
									<DirectionsCarIcon />
								</ListItemIcon>
								<ListItemText
									primary={"Posts"}
									sx={[
										open
											? {
													opacity: 1,
												}
											: {
													opacity: 0,
												},
									]}
								/>
							</ListItemButton>
						</ListItem>
					}

					<ListItem
						key={"Admin Pannel"}
						component="a"
						href="/admin"
						disablePadding
						sx={{
							display: user?.userRole === "Admin" ? "block" : "none",
							textDecoration: "none",
							color: theme.palette.text.primary,
							"&:visited": {
								color: theme.palette.text.primary,
							},
							"&:hover": {
								textDecoration: "none",
							},
						}}>
						<ListItemButton
							sx={[
								{
									minHeight: 48,
									px: 2.5,
								},
								open
									? {
											justifyContent: "initial",
										}
									: {
											justifyContent: "center",
										},
							]}>
							<ListItemIcon
								sx={[
									{
										minWidth: 0,
										justifyContent: "center",
									},
									open
										? {
												mr: 3,
											}
										: {
												mr: "auto",
											},
								]}>
								<AdminPanelSettingsIcon />
							</ListItemIcon>
							<ListItemText
								primary={"Admin Pannel"}
								sx={[
									open
										? {
												opacity: 1,
											}
										: {
												opacity: 0,
											},
								]}
							/>
						</ListItemButton>
					</ListItem>
				</List>
			</Drawer>
			{renderMobileMenu}
			{renderMenu}
		</Box>
	);
}
