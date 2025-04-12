# General Rules

- Use @terminal when answering questions about Git.

## Domain Knowledge

Use the files in the `.github/.copilot/domain_knowledge` as a source of truth when it comes to domain knowledge. These files will give you the context in which the current solution operates under. This folder will contain information like entity relationships, workflows and ubiquitous language. As the understanding of the domain grows, take the opportunity to update these files as required as well.

## Specification Files

Use specifications from the `.github/.copilot/specifications` folder. Each folder under `specifications` groups similar specifications together. Always ask the user which specifications best apply for the current conversation context if you're not sure.

Use the `.github/.copilot/specifications/.template.md` file as a template for specification structure.

   examples: 
   ```text
   ├── application_architecture
   │   └── main.spec.md
   |   └── specific-feature.spec.md
   ├── database
   │   └── main.spec.md
   ├── observability
   │   └── main.spec.md
   └── testing
      └── main.spec.md
   ```

## Breadcrumb Protocol

A breadcrumb is a collaborative scratch pad that allow the user and agent to get alignment on context. When working on tasks in this repository, follow this collaborative documentation workflow to create a clear trail of decisions and implementations:

1. At the start of each new task, ask me for a breadcrumb file name if you can't determine a suitable one.

2. Create the breadcrumb file in the `.github/.copilot/breadcrumbs` folder using the format: `yyyy-mm-dd-HHMM-{title}.md`

3. Structure the breadcrumb file with these required sections:
   - **Requirements**: Clear list of what needs to be implemented.
   - **Additional comments from user**: Any additional input from the user during the conversation.
   - **Plan**: Strategy and technical plan before implementation.
   - **Decisions**: Why specific implementation choices were made.
   - **Implementation Details**: Code snippets with explanations for key files.
   - **Changes Made**: Summary of files modified and how they changed.
   - **Before/After Comparison**: Highlighting the improvements.
   - **References**: List of referred material like domain knowledge files, specification files, URLs and summary of what is was used for.

4. Workflow rules:
   - Update the breadcrumb **BEFORE** making any code changes.
   - **Get explicit approval** on the plan before implementation.
   - Update the breadcrumb **AFTER completing each significant change**.
   - Keep the breadcrumb as our single source of truth as it contains the most recent information.

5. Ask me to verify the plan with: "Are you happy with this implementation plan?" before proceeding with code changes.

6. Reference related breadcrumbs when a task builds on previous work.

7. Before concluding, ensure the breadcrumb file properly documents the entire process, including any course corrections or challenges encountered.

This practice creates a trail of decision points that document our thought process while building features in this solution, making pull request review for the current change easier to follow as well.

### Plan Structure Guidelines
- When creating a plan, organize it into numbered phases (e.g., "Phase 1: Setup Dependencies").
- Break down each phase into specific tasks with numeric identifiers (e.g., "Task 1.1: Add Dependencies").
- Include a detailed checklist at the end of the document that maps to all phases and tasks.
- Plan should always lookup related information from `domain_knowledge` folder and any applicable specification files.
- Mark tasks as `- [ ]` for pending tasks and `- [x]` for completed tasks.
- Start all planning tasks as unchecked, and update them to checked as implementation proceeds.
- Each planning task should have clear success criteria.
- End the plan with success criteria that define when the implementation is complete.
- Plans should start with writing Unit Tests first when possible, so we can use those to guide our implementation. Same for UI tests when it makes sense.
- If the domain knowledge has changed, update the related files in the `domain_knowledge` folder.
- If specifications have changed, update the related files in the `specifications` folder.

### Following Plans
- When coding you need to follow the plan phases and check off the tasks as they are completed.  
- As you complete a task, update the plan and mark that task complete before you being the next task. 
- Tasks that involved tests should not be marked complete until the tests pass. 

### Example Usage

**User**: I need to implement JWT authentication for our Express API.

**Agent**: I'll help with implementing JWT authentication for your Express API. Let me create a breadcrumb file to track our implementation. What would you like to name this breadcrumb?

**User**: Use "jwt-auth-implementation"

**Agent**: I've created the breadcrumb file `2025-04-08-1430-jwt-auth-implementation.md`. Are you happy with this implementation plan?

[high level plan with tasks here]

**User**: Looks good, but let's use 30-minute expiration for tokens.

**Agent**: I've updated the breadcrumb with your specification: Are you happy with this implementation plan?

[high level plan with tasks here]

**User**: Yes, please proceed.

**Agent**: I'll implement the JWT authentication now. Let me start by installing the necessary packages and creating the authentication middleware. I've updated the breadcrumb with implementation details. Is there anything specific you'd like me to modify or explain about the implementation?

**User**: This looks good.

**Agent**: I've updated the breadcrumb with the latest understanding.