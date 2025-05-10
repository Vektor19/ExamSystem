import React from "react";
import { useNavigate } from "react-router";
import {
  Box,
  Zoom,
  Typography,
  Stack,
  Divider,
  Chip,
  Paper,
  Avatar,
} from "@mui/material";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import DashboardPaper from "../Papers/DashboardPaper";
import PrimaryFab from "../Buttons/PrimaryFab";
import LoadingPage from "../Extra/LoadingPage";
import { useExamSession } from "../../Providers/ExamSessionProvider";

const StudentExamResult: React.FC = () => {
  const { studentExam } = useExamSession();
  const navigate = useNavigate();

  if (!studentExam) {
    return <LoadingPage />;
  }

  const { name, startDate, endDate, createdBy, examUser } =
    studentExam;

  const formattedDate = (date: string) =>
    new Date(date).toLocaleString("uk-UA", {
      day: "2-digit",
      month: "2-digit",
      hour: "2-digit",
      minute: "2-digit",
    });

  return (
    <>
      <Typography variant="h4" fontWeight={600} gutterBottom>
        Exam: {name}
      </Typography>

      <DashboardPaper>
        <Stack spacing={3}>
          <Stack direction="row" alignItems="center" spacing={2}>
            <Avatar sx={{ width: 56, height: 56 }}>
              {examUser.firstName[0]}
              {examUser.lastName[0]}
            </Avatar>
            <Box>
              <Typography variant="h6">
                {examUser.firstName} {examUser.lastName}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                {examUser.email}
              </Typography>
            </Box>
          </Stack>

          <Divider />

          <Stack direction="row" spacing={2} flexWrap="wrap">
            <Chip
              label={`Grade: ${examUser.grade}`}
              color="primary"
              variant="filled"
              sx={{ fontSize: "1rem", borderRadius: 1 }}
            />
            <Chip
              label={`${
                Boolean(examUser.isBlocked)
                  ? "Completed with violations"
                  : Boolean(examUser.completeStatus)
                  ? "Completed"
                  : "Not completed"
              }`}
              color={
                Boolean(examUser.isBlocked)
                  ? "error"
                  : Boolean(examUser.completeStatus)
                  ? "success"
                  : "default"
              }
              variant="outlined"
              sx={{ borderRadius: 1 }}
            />
            <Chip
              label={`Start: ${formattedDate(startDate)}`}
              variant="outlined"
              sx={{ borderRadius: 1 }}
            />
            <Chip
              label={`End: ${formattedDate(endDate)}`}
              variant="outlined"
              sx={{ borderRadius: 1 }}
            />
            <Chip
              label={`Instructor: ${createdBy.firstName} ${createdBy.lastName}`}
              variant="outlined"
              sx={{ borderRadius: 1 }}
            />
          </Stack>

          {examUser.violations.length > 0 && (
            <>
              <Divider />
              <Typography variant="h6" color="error">
                Violations
              </Typography>
              <Stack spacing={2}>
                {examUser.violations.map((v, i) => (
                  <Paper
                    key={v.violationId}
                    elevation={1}
                    sx={{ p: 2, borderRadius: 1 }}
                  >
                    <Typography variant="subtitle1" fontWeight={600}>
                      {i + 1}. {v.violationType}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      {v.description}
                    </Typography>
                  </Paper>
                ))}
              </Stack>
            </>
          )}
        </Stack>
      </DashboardPaper>

      <Zoom in>
        <Box
          position="fixed"
          bottom={24}
          left={"calc(var(--dashboard-navigation-width) + 24px)"}
          zIndex={1300}
        >
          <PrimaryFab
            size="small"
            variant="extended"
            onClick={() => navigate(-1)}
          >
            <ArrowBackIcon sx={{ mr: 1 }} />
            Go Back
          </PrimaryFab>
        </Box>
      </Zoom>
    </>
  );
};

export default StudentExamResult;
