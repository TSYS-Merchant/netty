Feature: AddSomeNumbers
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario Outline: Add two numbers
	Given I have entered the first number <firstNumber> into the calculator
	And I have entered the second number <secondNumber> into the calculator
	When I call the web service to add the numbers
	Then the result should be <resultingNumber>

	Examples: 
	| firstNumber | secondNumber | resultingNumber |
	| 10          | 15           | 25              |
	| 5           | 1            | 6               |
	| 7           | -1           | 6               |

Scenario: Calling Hello World
	Given The AddTwoNumbers web service
	When I call the HelloWorld method
	Then I should receive the message "Hello World"

