import {
  Box,
  Typography,
  Stack,
  Paper,
  Chip,
  Divider,
  Zoom,
} from "@mui/material";
import { StudentExam } from "../../Models/StudentExam";
import { useExams } from "../../Providers/ExamsProvider";
import DashboardPaper from "../Papers/DashboardPaper";
import { useNavigate } from "react-router-dom";
import PrimaryButton from "../Buttons/PrimaryButton";
import PrimaryFab from "../Buttons/PrimaryFab";
import { useState } from "react";
import AddIcon from "@mui/icons-material/Add";
import JoinExamModal from "./JoinExamModal";
import ExamService from "../../Services/ExamService";
import { useUser } from "../../Providers/UserProvider";
import SecondaryButton from "../Buttons/SecondaryButton";

const StudentExamsBody: React.FC = () => {
  const { studentExams, fetchStudentExams } = useExams();
  const { user } = useUser();
  const [showJoinExamModal, setShowJoinExamModal] = useState(false);
  const navigate = useNavigate();

  const formatDuration = (start: string, end: string) => {
    const startDate = new Date(start);
    const endDate = new Date(end);
    const diffMs = endDate.getTime() - startDate.getTime();
    const mins = Math.floor(diffMs / 60000);
    return `${Math.floor(mins / 60)}h ${mins % 60}m`;
  };

  return (
    <>
      <JoinExamModal
        open={showJoinExamModal}
        onClose={() => setShowJoinExamModal(false)}
        onSave={async (joinCode: string) => {
          try {
            await ExamService.joinExam(user?.userId ?? "", joinCode);
            await fetchStudentExams();
          } catch (error) {
            console.error("Failed to join exam:", error);
          }
        }}
      />

      <DashboardPaper sx={{ p: 3 }}>
        <Stack spacing={2}>
          {studentExams?.map(
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
                        Questions: {exam.questionCount}
                      </Typography>
                      <Divider orientation="vertical" flexItem />
                      <Typography variant="body2" color="text.secondary">
                        Time provided:{" "}
                        {formatDuration(exam.startDate, exam.endDate)}
                      </Typography>
                      <Divider orientation="vertical" flexItem />
                      <Typography variant="body2" color="error">
                        Start Date:{" "}
                        {new Date(exam.startDate).toLocaleString("uk-UA", {
                          day: "2-digit",
                          month: "2-digit",
                          hour: "2-digit",
                          minute: "2-digit",
                        })}
                      </Typography>

                      <Divider orientation="vertical" flexItem />
                      <Typography variant="body2" color="error">
                        Deadline:{" "}
                        {new Date(exam.endDate).toLocaleString("uk-UA", {
                          day: "2-digit",
                          month: "2-digit",
                          hour: "2-digit",
                          minute: "2-digit",
                        })}
                      </Typography>
                      <Divider orientation="vertical" flexItem />
                      <Chip
                        sx={{ borderRadius: 1 }}
                        size="small"
                        label={exam.status}
                        color={
                          exam.status === "Closed"
                            ? "default"
                            : exam.status === "Started"
                            ? "primary"
                            : "success"
                        }
                      />
                    </Stack>
                  </Box>
                  {(Boolean(exam.examUser.completeStatus) && exam.status !== "NotStarted") && (
                    <PrimaryButton

                      onClick={() =>
                        navigate(`/dashboard/exam-result/${exam.examId}`)
                      }
                    >
                      Results
                    </PrimaryButton>
                  )}
                  {exam.status === "NotStarted" && (<SecondaryButton
                    disabled
                    
                    onClick={() =>
                      navigate(`/dashboard/exam-session/${exam.examId}`)
                    }
                  >
                    Start
                  </SecondaryButton>
                  )}
                  {(!Boolean(exam.examUser.completeStatus) && exam.status === "Started") && (
                    <PrimaryButton
                      onClick={() =>
                        navigate(`/dashboard/exam-session/${exam.examId}`)
                      }
                    >
                      Start
                    </PrimaryButton>
                  )}
                  {exam.status === "Closed" && (
                    <PrimaryButton
                      onClick={() =>
                        navigate(`/dashboard/exam-result/${exam.examId}`)
                      }
                    >
                      Results
                    </PrimaryButton>
                  )}
                </Paper>
              )
          )}
        </Stack>
      </DashboardPaper>
      <Zoom in>
        <Box position="fixed" left={"50%"} bottom={24} zIndex={1300}>
          <PrimaryFab
            size="small"
            variant="extended"
            onClick={() => setShowJoinExamModal(true)}
          >
            <AddIcon sx={{ mr: 1 }} />
            Join Exam
          </PrimaryFab>
        </Box>
      </Zoom>
    </>
  );
};

export default StudentExamsBody;
