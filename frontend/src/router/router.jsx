import { createBrowserRouter } from "react-router-dom";
import ComponentsLookup from "../components/componentsLookup";
import CheckoutPage from "../pages/CheckoutPages/CheckoutPage/CheckoutPage";
import PickUpPointModal from "../pages/CheckoutPages/PickUpPointModal/PickUpPointModal";

const routes = [
    {
      path: '/components',
      element: <ComponentsLookup />,
    },
    {
      path: '/checkout',
      element: <CheckoutPage />,
      children: [
        {
          path: 'pick-up',
          element: <PickUpPointModal />,
        },
      ],
    },
  ];

  const router = createBrowserRouter(routes);
  export default router;