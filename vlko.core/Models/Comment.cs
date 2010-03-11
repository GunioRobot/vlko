﻿using System;
using System.Collections.Generic;
using Castle.ActiveRecord;

namespace vlko.core.Models
{
    [ActiveRecord]
    public class Comment
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [PrimaryKey(PrimaryKeyType.GuidComb)]
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Property(Length = 255)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the actual version.
        /// </summary>
        /// <value>The actual version.</value>
        [Property]
        public virtual int ActualVersion { get; set; }

        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        /// <value>The created at.</value>
        [Property]
        public virtual DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>The level.</value>
        [Property(Length = 255)]
        public virtual string Level { get; set; }

        /// <summary>
        /// Gets or sets the parent version.
        /// </summary>
        /// <value>The parent version.</value>
        [Property]
        public virtual int ParentVersion { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        [BelongsTo("ContentId", Lazy = FetchWhen.OnInvoke)]
        public virtual Content Content { get; set; }

        /// <summary>
        /// Gets or sets the parent comment.
        /// </summary>
        /// <value>The parent comment.</value>
        [BelongsTo("ParentCommentId", Lazy = FetchWhen.OnInvoke)]
        public virtual Comment ParentComment { get; set; }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>The owner.</value>
        [BelongsTo("OwnerId")]
        public virtual User Owner { get; set; }

        /// <summary>
        /// Gets or sets the comment versions.
        /// </summary>
        /// <value>The comment versions.</value>
        [HasMany(typeof(CommentVersion), ColumnKey = "CommentId", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Lazy = true, Inverse = true)]
        public IList<CommentVersion> CommentVersions { get; set; }

    }
}


