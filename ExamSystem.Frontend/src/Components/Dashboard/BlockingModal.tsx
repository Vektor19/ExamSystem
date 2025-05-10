import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Typography,
  Box,
} from "@mui/material";
import PrimaryButton from "../Buttons/PrimaryButton";
import { useNavigate } from "react-router-dom";

interface Props {
  open: boolean;
}

const BlockingModal: React.FC<Props> = ({ open }) => {
  const navigate = useNavigate();

  const handleGoToExams = () => {
    navigate("/dashboard/exam-management");
  };

  return (
    <Dialog open={open} maxWidth="sm">
      <Box p={2}>
        <DialogTitle>You have been blocked</DialogTitle>
        <DialogContent>
          <Typography variant="body1" color="error" fontWeight="bold" gutterBottom>
            Your exam has been invalidated due to suspicious activity.
          </Typography>
        </DialogContent>
        <DialogActions>
          <PrimaryButton variant="contained" onClick={handleGoToExams}>
            Back to Exams
          </PrimaryButton>
        </DialogActions>
      </Box>
    </Dialog>
  );
};

export default BlockingModal;
