import { Exam } from "../../Models/Exam";
import { useExams } from "../../Providers/ExamsProvider";
import { useNavigate } from "react-router-dom";
import {
  Paper,
  Typography,
  Box,
  Stack,
  Card,
  CardContent,
  Fab,
} from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import styles from "../../Styles/ExaminatorExamsBody.module.css";
import PrimaryFab from "../Buttons/PrimaryFab";

const ExaminatorExamsBody: React.FC = () => {
  const { examinatorExams } = useExams();
  const navigate = useNavigate();

  return (
    <div className={styles["exams-page-container"]}>
      <Box minHeight="80vh">
        <Paper elevation={3} sx={{ p: 4, borderRadius: 3 }}>

          <Stack spacing={2}>
            {examinatorExams?.length ? (
              examinatorExams.map(
                (exam: Exam | null) =>
                  exam && (
                    <Card key={exam.examId} variant="outlined">
                      <CardContent>
                        <Typography variant="h6" gutterBottom>
                          {exam.name}
                        </Typography>
                        <Typography>Status: {exam.status}</Typography>
                        <Typography>Start Date: {exam.startDate}</Typography>
                        <Typography>End Date: {exam.endDate}</Typography>
                        <Typography>Questions: {exam.questionCount}</Typography>
                        <Typography>
                          Participants: {exam.participantCount}
                        </Typography>
                      </CardContent>
                    </Card>
                  )
              )
            ) : (
              <Typography variant="body1" color="text.secondary">
                No exams found.
              </Typography>
            )}
          </Stack>
        </Paper>
        <PrimaryFab
          color="primary"
          aria-label="add"
          size ="large"
          sx={{
            position: "fixed",
            bottom: 24,
            right: 24,
            zIndex: 1000,
          }}
          onClick={() => navigate("/dashboard/create-exam")}
        >
          <AddIcon />
        </PrimaryFab>
      </Box>
    </div>
  );
};

export default ExaminatorExamsBody;
