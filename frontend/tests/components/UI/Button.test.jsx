import React from 'react';
import {fireEvent, render, screen} from '@testing-library/react';
import '@testing-library/jest-dom';
import { Button } from '../../../src/components/UI/Button/Button.jsx';
import { Icon } from '../../../src/components/UI/Icon/Icon.jsx';
import {expect, it} from "vitest";

//TODO: у меня почему-то иногда сразу интеграционные тесты, а иногда unit тесты с моками вложенных компонентов,
// надо бы разделить когда-нибудь
vi.mock('../../../src/components/UI/Icon/Icon.jsx', () => ({
    Icon: vi.fn(() => <div data-testid="icon-mock" />),
}));


describe('Button Component', () => {
    it('renders with text', () => {
        render(<Button text="Click me" />);
        const textElement = screen.getByText('Click me');
        expect(textElement).toBeInTheDocument();
    });

    it('renders with icon when iconPath is provided', () => {
        render(<Button iconPath="test-icon.png" />);
        const iconElement = screen.getByTestId('icon-mock');
        expect(iconElement).toBeInTheDocument();
        expect(Icon).toHaveBeenCalledWith(expect.objectContaining({ path: 'test-icon.png', size: 'default' }), {});
    });

    it('applies color class', () => {
        render(<Button color="yellow" />);
        const button = screen.getByRole('button');
        expect(button).toHaveClass('button-yellow');
    });

    it('applies round class when round is true', () => {
        render(<Button round />);
        const button = screen.getByRole('button');
        expect(button).toHaveClass('button-rounded');
    });

    it('applies shadow class when shadow is true', () => {
        render(<Button shadow />);
        const button = screen.getByRole('button');
        expect(button).toHaveClass('button-shadow');
    });

    it('applies big class when big is true', () => {
        render(<Button big />);
        const button = screen.getByRole('button');
        expect(button).toHaveClass('button-big');
    });

    it('applies additional className', () => {
        render(<Button className="custom-class" />);
        const button = screen.getByRole('button');
        expect(button).toHaveClass('custom-class');
    });

    it('does not render text element when text is empty, null or undefined', () => {
        let {container} = render(<Button text={''} />);
        expect(container.querySelector('p')).not.toBeInTheDocument();
        ({container} = render(<Button text={null} />));
        expect(container.querySelector('p')).not.toBeInTheDocument();
        ({container} = render(<Button text={undefined} />));
        expect(container.querySelector('p')).not.toBeInTheDocument();
    });

    it('does not render icon when iconPath is empty, null or undefined', () => {
        render(<Button iconPath="" />);
        expect(screen.queryByTestId('icon-mock')).not.toBeInTheDocument();
        render(<Button iconPath={null} />);
        expect(screen.queryByTestId('icon-mock')).not.toBeInTheDocument();
        render(<Button iconPath={undefined} />);
        expect(screen.queryByTestId('icon-mock')).not.toBeInTheDocument();
    });

    it('calls function when it was passed through onClick prop and button is clicked', () => {
        const buttonFunction = vi.fn();
        render(<Button onClick={buttonFunction} />);
        fireEvent.click(screen.getByRole('button'));
        expect(buttonFunction).toHaveBeenCalled();
    });
});
