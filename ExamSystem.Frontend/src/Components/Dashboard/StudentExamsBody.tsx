import {
  Box,
  Typography,
  Stack,
  Paper,
  Button,
  Chip,
  Divider,
} from "@mui/material";
import { StudentExam } from "../../Models/StudentExam";
import { useExams } from "../../Providers/ExamsProvider";
import DashboardPaper from "../Papers/DashboardPaper";
import { useNavigate } from "react-router-dom";

const StudentExamsBody: React.FC = () => {
  const { examinatorExams } = useExams();
  const navigate = useNavigate();

  const formatDuration = (start: string, end: string) => {
    const startDate = new Date(start);
    const endDate = new Date(end);
    const diffMs = endDate.getTime() - startDate.getTime();
    const mins = Math.floor(diffMs / 60000);
    return `${Math.floor(mins / 60)}h ${mins % 60}m`;
  };

  return (
    <DashboardPaper sx={{ p: 3 }}>
      <Typography variant="h5" fontWeight={600} mb={3}>
        Доступні іспити
      </Typography>

      <Stack spacing={2}>
        {examinatorExams?.map(
          (exam: StudentExam | null) =>
            exam && (
              <Paper
                key={exam.examId}
                elevation={3}
                sx={{
                  p: 2,
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "space-between",
                  borderLeft: `6px solid ${
                    exam.status === "Finished"
                      ? "#9e9e9e"
                      : exam.status === "Started"
                      ? "#1976d2"
                      : "#4caf50"
                  }`,
                }}
              >
                <Box sx={{ flexGrow: 1 }}>
                  <Typography variant="h6" fontWeight={600}>
                    {exam.name}
                  </Typography>

                  <Stack direction="row" spacing={2} mt={1} flexWrap="wrap">
                    <Typography variant="body2" color="text.secondary">
                      Питань: {exam.questionCount}
                    </Typography>
                    <Divider orientation="vertical" flexItem />
                    <Typography variant="body2" color="text.secondary">
                      Час: {formatDuration(exam.startDate, exam.endDate)}
                    </Typography>
                    <Divider orientation="vertical" flexItem />
                    <Typography variant="body2" color="error">
                      Дедлайн:{" "}
                      {new Date(exam.endDate).toLocaleString("uk-UA", {
                        day: "2-digit",
                        month: "2-digit",
                        hour: "2-digit",
                        minute: "2-digit",
                      })}
                    </Typography>
                    <Divider orientation="vertical" flexItem />
                    <Chip
                      size="small"
                      label={exam.status}
                      color={
                        exam.status === "Finished"
                          ? "default"
                          : exam.status === "Started"
                          ? "primary"
                          : "success"
                      }
                    />
                  </Stack>
                </Box>

                <Button
                  variant="contained"
                  sx={{ ml: 3 }}
                  disabled={exam.status !== "NotStarted"}
                  onClick={() => navigate(`/exam/${exam.examId}/start`)}
                >
                  Start
                </Button>
              </Paper>
            )
        )}
      </Stack>
    </DashboardPaper>
  );
};

export default StudentExamsBody;
