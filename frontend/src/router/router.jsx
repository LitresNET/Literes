import { createBrowserRouter } from "react-router-dom";
import ComponentsLookup from "../components/componentsLookup";
import CheckoutPage from "../pages/CheckoutPages/CheckoutPage/CheckoutPage";
import CustomSubscriptionPage from "../pages/SubscriptionPages/CustomSubscriptionPage/CustomSubscriptionPage";
import MainLayout from "../layouts/MainLayout/MainLayout";
import WelcomePage from "../pages/MainPages/WelcomePage/WelcomePage";
import SignInPage from "../pages/AuthPages/SignInPage/SignInPage.jsx";
import SignUpPage from "../pages/AuthPages/SignUpPage/SignUpPage.jsx";
import SearchPage from "../pages/MainPages/SearchPage/SearchPage";
import SubscriptionPage from "../pages/SubscriptionPages/SubscriptionPage/SubscriptionPage";

const routes = [
    {
      path: '/components',
      element: <ComponentsLookup />,
    },
    {
      path: '/',
      element: <MainLayout />,
      children: [
        {
          path: 'checkout',
          element: <CheckoutPage />,
        },
        {
          path: 'signin',
          element: <SignInPage />
        },
        {
          path: 'signup',
          element: <SignUpPage />
        },
        {
          path: 'search',
          element: <SearchPage />
        },
        {
          path: ''
        },
        {
          path: 'subscription',
          element: <SubscriptionPage />
        },
        {
          path: 'subscription/custom',
          element: <CustomSubscriptionPage />
        },
          {
            path:'',
              element: <WelcomePage />
          }
      ]
    },
  ];

const router = createBrowserRouter(routes);

export default router;
