import { createBrowserRouter } from "react-router-dom";
import ComponentsLookup from "../components/componentsLookup";
import CheckoutPage from "../pages/CheckoutPages/CheckoutPage/CheckoutPage";
import CustomSubscriptionPage from "../pages/SubscriptionPages/CustomSubscriptionPage/CustomSubscriptionPage";
import MainLayout from "../layouts/MainLayout/MainLayout";
import WelcomePage from "../pages/MainPages/WelcomePage/WelcomePage";
import SubscriptionPage from "../pages/SubscriptionPages/SubscriptionPage/SubscriptionPage";

const routes = [
    {
      path: '/components',
      element: <ComponentsLookup />,
    },
    {
      path: '/',
      element: <MainLayout/>,
      children: [
        {
          path: 'checkout',
          element: <CheckoutPage />,
        },
        {
          path: 'subscription/custom',
          element: <CustomSubscriptionPage/>
        },
        {
          path:'home',
            element: <WelcomePage />
        },
        {
          path: 'subscription',
          element: <SubscriptionPage />
        }
      ]
    },
  ];

const router = createBrowserRouter(routes);

export default router;
