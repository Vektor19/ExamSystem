import { TextField, Paper } from "@mui/material";
import { useUser } from "../../Providers/UserProvider";
import profileStyles from "../../Styles/Profile.module.css";
import { useState, useEffect } from "react";
import PrimaryButton from "../Buttons/PrimaryButton";
import SecondaryButton from "../Buttons/SecondaryButton";

const Profile = () => {
  const { user, updateUser } = useUser();

  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
  });

  useEffect(() => {
    if (user) {
      setFormData({
        firstName: user.firstName,
        lastName: user.lastName,
        email: user.email,
      });
    }
  }, [user]);

  const handleChange = (e: any) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSave = async () => {
    await updateUser(formData);
  };

  const handleCancel = () => {
    setFormData({
      firstName: user?.firstName || "",
      lastName: user?.lastName || "",
      email: user?.email || "",
    });
  };

  return (
    <>
      <div className={profileStyles["profile-page-container"]}>
        <Paper elevation={2} className={profileStyles["profile-photo"]}>
          <img
            src="https://www.transparentpng.com/thumb/user/gray-user-profile-icon-png-fP8Q1P.png"
            alt="Profile"
          />
        </Paper>

        <Paper elevation={2} className={profileStyles["profile-info"]}>
          <div className={profileStyles["profile-info-item"]}>
            <TextField
              label="First Name"
              name="firstName"
              value={formData.firstName}
              onChange={handleChange}
              variant="outlined"
              fullWidth
            />
            <TextField
              label="Last Name"
              name="lastName"
              value={formData.lastName}
              onChange={handleChange}
              variant="outlined"
              fullWidth
            />
            <TextField
              label="Email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              variant="outlined"
              fullWidth
            />
          </div>
        </Paper>

        <Paper elevation={2} className={profileStyles["profile-extra"]}>
          Additional information or stats here
        </Paper>

        <Paper elevation={2} className={profileStyles["profile-actions"]}>
          <div className={profileStyles["profile-actions-buttons"]}>
            <PrimaryButton variant="contained" onClick={handleSave}>
              Save
            </PrimaryButton>
            <SecondaryButton variant="contained" onClick={handleCancel}>
              Cancel
            </SecondaryButton>
          </div>
        </Paper>
      </div>
    </>
  );
};

export default Profile;
