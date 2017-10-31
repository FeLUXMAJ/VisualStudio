﻿using System;
using System.ComponentModel.Composition;
using GitHub.Extensions;
using GitHub.InlineReviews.Services;
using GitHub.Services;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace GitHub.InlineReviews.Tags
{
    /// <summary>
    /// Factory class for <see cref="InlineCommentTagger"/>s.
    /// </summary>
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("text")]
    [TagType(typeof(ShowInlineCommentTag))]
    class InlineCommentTaggerProvider : IViewTaggerProvider
    {
        readonly IGitService gitService;
        readonly IGitClient gitClient;
        readonly IDiffService diffService;
        readonly IPullRequestSessionManager sessionManager;

        [ImportingConstructor]
        public InlineCommentTaggerProvider(
            IGitService gitService,
            IGitClient gitClient,
            IDiffService diffService,
            IPullRequestSessionManager sessionManager)
        {
            Guard.ArgumentNotNull(gitService, nameof(gitService));
            Guard.ArgumentNotNull(gitClient, nameof(gitClient));
            Guard.ArgumentNotNull(sessionManager, nameof(sessionManager));

            this.gitService = gitService;
            this.gitClient = gitClient;
            this.diffService = diffService;
            this.sessionManager = sessionManager;
        }

        public ITagger<T> CreateTagger<T>(ITextView view, ITextBuffer buffer) where T : ITag
        {
            return buffer.Properties.GetOrCreateSingletonProperty(() =>
                new InlineCommentTagger(
                    view,
                    buffer,
                    sessionManager)) as ITagger<T>;
        }
    }
}
