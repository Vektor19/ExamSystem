import { Fab, FabProps } from "@mui/material";
import { styled } from "@mui/material/styles";

const PrimaryFab = styled((props: FabProps) => (
  <Fab aria-label={props["aria-label"] ?? "add"} {...props} />
))(({ theme }) => ({
  fontFamily: '"Lato", sans-serif',
  textTransform: "none",
  transition: "all 0.3s ease-in-out",
  "&.MuiFab-circular": {
    backgroundColor: theme.palette.primary.main,
    color: theme.palette.primary.light,

    "&:hover": {
      backgroundColor: theme.palette.primary.light,
      color: theme.palette.hoverTextPrimaryColor.main,
    },
  },
  "&.MuiFab-sizeLarge": {
    fontSize: "1.5em",
    padding: "1.5em",
  },
  "&.MuiFab-sizeMedium": {
    
    fontSize: "1.2em",
    padding: "1.5em",
  },
  "&.MuiFab-sizeSmall": {
    fontSize: "1em",
    padding: "1.5em",
  },
}));

export default PrimaryFab;
