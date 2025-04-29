import { Button, ButtonProps } from "@mui/material";
import { styled } from "@mui/material/styles";

const PrimaryButton = styled((props: ButtonProps) => (
  <Button variant={props.variant ?? "contained"} {...props} />
))(({ theme }) => ({
  fontFamily: '"Lato", sans-serif',
  textTransform: "none",
  transition: "all 0.3s ease-in-out",
  "&.MuiButton-contained": {
    backgroundColor: theme.palette.primary.main,
    color: theme.palette.primary.light,

    "&:hover": {
      backgroundColor: theme.palette.primary.light,
      color: theme.palette.hoverTextPrimaryColor.main,
    },
  },

  "&.MuiButton-outlined": {
    backgroundColor: "transparent",
    color: theme.palette.primary.main,
    border: `2px solid ${theme.palette.primary.main}`,

    "&:hover": {
      backgroundColor: theme.palette.primary.main,
      color: theme.palette.primary.light,
    },
  },

  "&.MuiButton-sizeLarge": {
    fontSize: "1.2em",
    padding: "0.6em 1.5em",
  },
  "&.MuiButton-sizeMedium": {
    fontSize: "1em",
    padding: "0.5em 1.2em",
  },
  "&.MuiButton-sizeSmall": {
    fontSize: "0.8em",
    padding: "0.4em 1em",
  },
}));

export default PrimaryButton;
