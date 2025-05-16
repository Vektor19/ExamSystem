import { Paper, PaperProps } from "@mui/material";
import { styled } from "@mui/material/styles";
import studentDashboardStyles from "../../Styles/StudentDashboardBody.module.css";

const DashboardPaper = styled(({ className, ...props }: PaperProps) => (
  <Paper
    {...props}
    className={`${className} ${studentDashboardStyles["dashboard-paper"]}`}
    elevation={props.elevation ?? 3}
  />
))(({ }) => ({
  fontFamily: '"Lato", sans-serif',
  backgroundColor: "white",
  textTransform: "none",
  borderRadius: "0.4rem",
}));

export default DashboardPaper;
