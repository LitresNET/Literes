import React from 'react';
import { render } from '@testing-library/react';
import '@testing-library/jest-dom';
import { Quotation } from '../../../src/components/UI/Quotation/Quotation.jsx';

describe('Quotation component', () => {
    it('renders children correctly', () => {
        const text = 'This is a quotation';
        const { getByText } = render(<Quotation>{text}</Quotation>);

        const quotationElement = getByText(text);
        expect(quotationElement).toBeInTheDocument();
    });

    it('applies mini size class', () => {
        const { container } = render(<Quotation />);
        const quotationElement = container.firstChild;

        expect(quotationElement).toHaveClass('description-mini');
    });

    it('forwards additional props to Description', () => {
        const customClass = 'custom-class';
        const { container } = render(<Quotation className={customClass} />);
        const quotationElement = container.firstChild;

        expect(quotationElement).toHaveClass(customClass);
    });

    it('applies shadow class when shadow is true', () => {
        const { container } = render(<Quotation shadow />);
        const quotationElement = container.firstChild;

        expect(quotationElement).toHaveClass('banner-shadow');
    });
});
