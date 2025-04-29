import LoginForm from "../AuthForms/LoginForm";
import outletStyles from "../../Styles/PublicOutlet.module.css";
import loginStyles from "../../Styles/LoginPage.module.css";

const LoginPage = () => {
  return (
    <section className={`${outletStyles["public-outlet"]}`}>
      <div className={`${loginStyles["login-page-container"]} `}>
        <div className={`${loginStyles["form-container"]} `}>
            <LoginForm />
        </div>
      </div>
    </section>
  );
};

export default LoginPage;
