import React from 'react';
import {getByTestId, render} from '@testing-library/react';
import '@testing-library/jest-dom';
import { Description } from '../../../src/components/UI/Description/Description.jsx';

describe('Description component', () => {
    it('renders text correctly', () => {
        const text = 'This is a description';
        const { getByText } = render(<Description>{text}</Description>);

        const descriptionElement = getByText(text);
        expect(descriptionElement).toBeInTheDocument();
    });

    it('renders same children correctly', () => {
        const { getByTestId } = render(
            <Description>
                <Description data-testid={"same-child"}></Description>
            </Description>);

        expect(getByTestId("same-child")).toBeInTheDocument();
    });

    it('renders multiple children correctly', () => {
        const { getByText } = render(
            <Description>
                <div>Child 1</div>
                <div>Child 2</div>
            </Description>);

        expect(getByText("Child 1")).toBeInTheDocument();
        expect(getByText("Child 2")).toBeInTheDocument();
    });

    it('applies default classes', () => {
        const { container } = render(<Description />);
        const descriptionElement = container.firstChild;

        expect(descriptionElement).toHaveClass('banner description');
    });

    it('applies mini size class when size is "mini"', () => {
        const { container } = render(<Description size="mini" />);
        const descriptionElement = container.firstChild;

        expect(descriptionElement).toHaveClass('description-mini');
    });

    it('applies shadow class when shadow is true', () => {
        const { container } = render(<Description shadow />);
        const descriptionElement = container.firstChild;

        expect(descriptionElement).toHaveClass('banner-shadow');
    });

    it('applies additional className', () => {
        const customClass = 'custom-class';
        const { container } = render(<Description className={customClass} />);
        const descriptionElement = container.firstChild;

        expect(descriptionElement).toHaveClass(customClass);
    });
});
