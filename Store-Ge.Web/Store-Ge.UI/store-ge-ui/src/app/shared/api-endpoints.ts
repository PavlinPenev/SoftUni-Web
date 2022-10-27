import { environment } from 'src/environments/environment';

const compose =
  (...fns: any) =>
  (x: any) =>
    fns.reduceRight((v: any, f: any) => f(v), x);
const join = (separator: string) => (left: string) => (right: string) =>
  `${left}${separator}${right}`;

const joinWithSlash = join('/');
const prependWithBaseUrl = joinWithSlash(environment.webApiBaseUrl);

// Accounts Controller
const prependAccountsControllerRoute = joinWithSlash('api/accounts');
const prependBaseUrlAndAccountsControllerRoute = compose(
  prependWithBaseUrl,
  prependAccountsControllerRoute
);

export const REGISTER_ENDPOINT =
  prependBaseUrlAndAccountsControllerRoute('register');
export const LOGIN_ENDPOINT = prependBaseUrlAndAccountsControllerRoute('login');
export const REFRESH_ACCESS_TOKEN_ENDPOINT =
  prependBaseUrlAndAccountsControllerRoute('refresh-token');
export const RESEND_CONFIRMATION_EMAIL_ENDPOINT =
  prependBaseUrlAndAccountsControllerRoute('resend-confirmation-email');
export const CONFIRM_EMAIL_ENDPOINT =
  prependBaseUrlAndAccountsControllerRoute('confirm-email');
export const FORGOT_PASSWORD_ENDPOINT =
  prependBaseUrlAndAccountsControllerRoute('forgot-password');
export const RESET_PASSWORD_ENDPOINT =
  prependBaseUrlAndAccountsControllerRoute('password-reset');
