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
export const ADD_CASHIER_ENDPOINT =
  prependBaseUrlAndAccountsControllerRoute('add-cashier');
export const GET_USER_ENDPOINT =
  prependBaseUrlAndAccountsControllerRoute('get-user');
export const UPDATE_USER_ENDPOINT =
  prependBaseUrlAndAccountsControllerRoute('update-user');

// Stores Controller
const prependStoresControllerRoute = joinWithSlash('api/stores');
const prependBaseUrlAndStoresControllerRoute = compose(
  prependWithBaseUrl,
  prependStoresControllerRoute
);

export const GET_USER_STORES_ENDPOINT =
  prependBaseUrlAndStoresControllerRoute('get-stores');
export const GET_STORE_ENDPOINT =
  prependBaseUrlAndStoresControllerRoute('get-store');
export const ADD_STORE_ENDPOINT =
  prependBaseUrlAndStoresControllerRoute('add-store');
export const EXPORT_STORE_REPORT_ENDPOINT =
  prependBaseUrlAndStoresControllerRoute('export-report');

// Products Controller
const prependProductsControllerRoute = joinWithSlash('api/products');
const prependBaseUrlAndProductsControllerRoute = compose(
  prependWithBaseUrl,
  prependProductsControllerRoute
);

export const GET_STORE_PRODUCTS_ENDPOINT =
  prependBaseUrlAndProductsControllerRoute('get-store-products');
export const GET_STORE_ADD_PRODUCTS_ENDPOINT =
  prependBaseUrlAndProductsControllerRoute('get-store-add-products');
export const SELL_PRODUCTS_ENDPOINT =
  prependBaseUrlAndProductsControllerRoute('sell-products');

// Orders Controller
const prependOrdersControllerRoute = joinWithSlash('api/orders');
const prependBaseUrlAndOrdersControllerRoute = compose(
  prependWithBaseUrl,
  prependOrdersControllerRoute
);

export const GET_USER_ORDERS_ENDPOINT =
  prependBaseUrlAndOrdersControllerRoute('get-user-orders');
export const GET_STORE_ORDERS_ENDPOINT =
  prependBaseUrlAndOrdersControllerRoute('get-store-orders');
export const ADD_ORDER_ENDPOINT =
  prependBaseUrlAndOrdersControllerRoute('add-order');

// Suppliers Controller
const prependSuppliersControllerRoute = joinWithSlash('api/suppliers');
const prependBaseUrlAndSuppliersControllerRoute = compose(
  prependWithBaseUrl,
  prependSuppliersControllerRoute
);

export const GET_USER_SUPPLIERS_ENDPOINT =
  prependBaseUrlAndSuppliersControllerRoute('get-user-suppliers');
export const GET_USER_SUPPLIERS_PAGED_ENDPOINT =
  prependBaseUrlAndSuppliersControllerRoute('get-user-suppliers-paged');
export const ADD_SUPPLIER_ENDPOINT =
  prependBaseUrlAndSuppliersControllerRoute('add-supplier');
