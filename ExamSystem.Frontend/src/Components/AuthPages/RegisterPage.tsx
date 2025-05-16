import RegisterForm from "../AuthForms/RegisterForm";
import outletStyles from "../../Styles/PublicOutlet.module.css";
import loginStyles from "../../Styles/LoginPage.module.css";

const RegisterPage = () => {
  return (
    <section className={`${outletStyles["public-outlet"]}`}>
      <div className={`${loginStyles["login-page-container"]} `}>
        <div className={`${loginStyles["form-container"]} `}>
          <RegisterForm />
        </div>
      </div>
    </section>
  );
};

export default RegisterPage;
